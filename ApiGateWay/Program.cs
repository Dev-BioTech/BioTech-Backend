using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Load the ocelot.json configuration file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// 2. Add Ocelot services to the container
builder.Services.AddOcelot(builder.Configuration);

// Add other services if needed (e.g., for authentication at the gateway)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Add DB Context
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL_MODE")};";

builder.Services.AddDbContext<ApiGateWay.Data.AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

// JWT Authentication
var secretKey = builder.Configuration["JwtConfig:Secret"];

// Fallback: Check for JWT_SECRET directly environment variable (useful if mapping fails)
if (string.IsNullOrEmpty(secretKey))
{
    secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
}

var issuer = builder.Configuration["JwtConfig:Issuer"];
if (string.IsNullOrEmpty(issuer)) issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

var audience = builder.Configuration["JwtConfig:Audience"];
if (string.IsNullOrEmpty(audience)) audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 16)
{
    throw new ArgumentException("JWT Secret is missing or too short. Checked 'JwtConfig:Secret' and 'JWT_SECRET' env var.");
}

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 1. Map Controllers
// This registers the /api/auth endpoints
app.MapControllers();

// 2. Use Ocelot Middleware conditionally
// We tell ASP.NET: "If the request does NOT start with /api/v1/Auth, then use Ocelot"
// This prevents Ocelot from eating our local login requests.
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api/v1/Auth", StringComparison.OrdinalIgnoreCase), 
    appBuilder =>
{
    appBuilder.UseOcelot().Wait();
});

app.Run();