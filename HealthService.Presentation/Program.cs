using DotNetEnv;
using HealthService.Application;
using HealthService.Infrastructure;
using HealthService.Presentation.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Configure Port for Railway
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(int.Parse(port));
    });
}

// Configure Database Connection
// ------------------------------------------------------------------------------------------------
// [STANDARD CONFIGURATION]
// Database, JWT, and Gateway Configuration - Unified for Local and PaaS (Clever Cloud/Railway)
// ------------------------------------------------------------------------------------------------

// 1. Database Connection
var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI") ?? 
                       Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                       Environment.GetEnvironmentVariable("DB_URL");

if (!string.IsNullOrEmpty(connectionString))
{
    if (connectionString.StartsWith("postgresql://"))
    {
        try 
        {
            var uri = new Uri(connectionString);
            var userInfo = uri.UserInfo.Split(':');
            var host = uri.Host;
            var parsedPort = uri.Port > 0 ? uri.Port : 5432;
            var path = uri.AbsolutePath.TrimStart('/');
            var user = userInfo.Length > 0 ? userInfo[0] : "";
            var pass = userInfo.Length > 1 ? userInfo[1] : "";
            
            connectionString = $"Host={host};Port={parsedPort};Database={path};Username={user};Password={pass};Ssl Mode=Require;Trust Server Certificate=true;";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Config Error] Failed to parse URI: {ex.Message}");
        }
    }
}
else
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    if (!string.IsNullOrEmpty(dbHost))
    {
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
        var dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME") ?? "biotech_db";
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

        connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};Trust Server Certificate=true;";
    }
}

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// 2. JWT Configuration
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (!string.IsNullOrEmpty(jwtSecret)) builder.Configuration["Jwt:Secret"] = jwtSecret;

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
if (!string.IsNullOrEmpty(jwtIssuer)) builder.Configuration["Jwt:Issuer"] = jwtIssuer;

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
if (!string.IsNullOrEmpty(jwtAudience)) builder.Configuration["Jwt:Audience"] = jwtAudience;

// 3. Gateway Secret
var gatewaySecret = Environment.GetEnvironmentVariable("GATEWAY_SECRET");
if (!string.IsNullOrEmpty(gatewaySecret)) builder.Configuration["Gateway:Secret"] = gatewaySecret;

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// Add Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<HealthService.Presentation.Services.GatewayAuthenticationService>();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Health Service API", Version = "v1" });
    
    c.AddSecurityDefinition("Gateway", new OpenApiSecurityScheme
    {
        Description = "Gateway Secret for direct access (X-Gateway-Secret header)",
        Name = "X-Gateway-Secret",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Gateway"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Gateway"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register Messenger
builder.Services.AddHttpClient();
builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();

// Register Gateway Auth
builder.Services.AddScoped<HealthService.Presentation.Services.GatewayAuthenticationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "HealthService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();