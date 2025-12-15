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

// CORS Configuration
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',', ';') ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            if (allowedOrigins.Contains("*"))
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }
            else
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }
        });
});

// 1. Add DB Context
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL_MODE")};";

// JWT Authentication
var secretKey = builder.Configuration["JwtConfig:Secret"];

// Port for deploy
// Port for deploy
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

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

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

// 1. Map Controllers
// This registers the /api/auth endpoints
app.MapControllers();

// 2. Use Ocelot Middleware
app.UseOcelot().Wait();

app.Run();