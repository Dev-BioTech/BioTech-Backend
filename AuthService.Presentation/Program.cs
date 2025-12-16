using AuthService.Application;
using AuthService.Infrastructure;

// Enable legacy timestamp behavior to handle DateTime Kind (UTC/Unspecified) issues
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AuthService", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => 
        {
            // Allow localhost for development
            if (origin.StartsWith("http://localhost")) return true;
            // Allow any Vercel subdomain
            if (origin.EndsWith(".vercel.app")) return true;
            // Allow API Gateway
            if (origin.Contains("railway.app")) return true;
            return false;
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// Configure Port for Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Configure Database Connection from Environment Variables (Railway)
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_DATABASE");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

    var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// Configure JWT from Environment Variables
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (!string.IsNullOrEmpty(jwtSecret)) builder.Configuration["Jwt:Secret"] = jwtSecret;

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
if (!string.IsNullOrEmpty(jwtIssuer)) builder.Configuration["Jwt:Issuer"] = jwtIssuer;

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
if (!string.IsNullOrEmpty(jwtAudience)) builder.Configuration["Jwt:Audience"] = jwtAudience;

// Add Layer Dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Authentication and JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for internal service mesh

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();