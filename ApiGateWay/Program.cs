using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Port for Railway
// Railway provides the PORT environment variable. We use this to tell Kestrel where to listen.
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// 2. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configure CORS (Merged User Request)
// Allows specific Vercel subdomains + localhost for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercel", policy =>
    {
        policy.WithOrigins(
            "https://*.vercel.app",  // Allow all Vercel subdomains (Note: WithOrigins with wildcards requires SetIsOriginAllowedToAllowWildcardSubdomains)
            "http://localhost:5000",
            "http://localhost:5001",
            "http://localhost:5002",
            "http://localhost:5003",
            "http://localhost:5004"
        )
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// 4. Ocelot Configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// 5. Database Context (Required if Gateway accesses DB directly)
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL_MODE")};";

// If you have a DbContext, ensure it is registered here, e.g.:
// builder.Services.AddDbContext<ApiGateWay.Data.AuthDbContext>(options => options.UseNpgsql(connectionString));

// 6. JWT Authentication
var secretKey = builder.Configuration["JwtConfig:Secret"];
if (string.IsNullOrEmpty(secretKey)) secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");

var issuer = builder.Configuration["JwtConfig:Issuer"];
if (string.IsNullOrEmpty(issuer)) issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

var audience = builder.Configuration["JwtConfig:Audience"];
if (string.IsNullOrEmpty(audience)) audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (!string.IsNullOrEmpty(secretKey) && secretKey.Length >= 16)
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
        };
    });
}

var app = builder.Build();

// 7. Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Not needed on Railway, handled at edge

// 8. Use CORS (Must be before Auth)
app.UseCors("AllowVercel");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 9. Use Ocelot
app.UseOcelot().Wait();

app.Run();