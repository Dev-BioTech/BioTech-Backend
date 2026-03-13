using DotNetEnv;
using FeedingService.Presentation.Services;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Shared.Infrastructure.Extensions;
using FeedingService.Infrastructure.Persistence;
using FeedingService.Application;
using FeedingService.Infrastructure;
using FeedingService.Presentation.Middlewares;
using Shared.Infrastructure.Middlewares;
using FeedingService.Application.Commands.CreateFeedingEvent;

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

// ------------------------------------------------------------------------------------------------
// [STANDARD CONFIGURATION]
// Database, JWT, and Gateway Configuration - Unified for Local and PaaS (Clever Cloud/Railway)
// ------------------------------------------------------------------------------------------------

// 1. Database Connection
var configConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "";

// Helper to get non-empty environment variable
string GetEnv(params string[] names) {
    foreach (var name in names) {
        var val = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrEmpty(val)) return val;
    }
    return null;
}

// Priority 1: Individual variables (Ensures port overrides like 50013 are used)
var dbHost = GetEnv("FEEDING_DB_HOST", "POSTGRESQL_ADDON_HOST", "DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = GetEnv("FEEDING_DB_PORT", "POSTGRESQL_ADDON_PORT", "DB_PORT") ?? "5432";
    var dbName = GetEnv("FEEDING_DB_NAME", "POSTGRESQL_ADDON_DB", "DB_DATABASE", "DB_NAME") ?? "biotech_db";
    var dbUser = GetEnv("FEEDING_DB_USER", "POSTGRESQL_ADDON_USER", "DB_USER");
    var dbPassword = GetEnv("FEEDING_DB_PASSWORD", "POSTGRESQL_ADDON_PASSWORD", "DB_PASSWORD");
    var dbSslMode = GetEnv("DB_SSL_MODE") ?? "Require";

    if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
    {
        connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};Trust Server Certificate=true;";
        Console.WriteLine($"[Config] Using individual variables (Host: {dbHost}, Port: {dbPort})");
    }
}

// Priority 2: Direct POSTGRESQL_ADDON_URI
if (string.IsNullOrEmpty(connectionString))
{
    var addonUri = GetEnv("POSTGRESQL_ADDON_URI", "DATABASE_URL", "DB_URL");
    if (!string.IsNullOrEmpty(addonUri) && addonUri.StartsWith("postgresql://"))
    {
        try 
        {
            var uri = new Uri(addonUri);
            var userInfo = uri.UserInfo.Split(':');
            var host = uri.Host;
            var parsedPort = uri.Port > 0 ? uri.Port : 5432;
            var path = uri.AbsolutePath.TrimStart('/');
            var user = userInfo.Length > 0 ? userInfo[0] : "";
            var pass = userInfo.Length > 1 ? userInfo[1] : "";
            
            connectionString = $"Host={host};Port={parsedPort};Database={path};Username={user};Password={pass};Ssl Mode=Require;Trust Server Certificate=true;";
            Console.WriteLine($"[Config] Using Addon URI (Host: {host}, Port: {parsedPort})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Config Error] Failed to parse URI: {ex.Message}");
        }
    }
}

// Priority 3: Fallback to configuration
if (string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(configConnectionString) && !configConnectionString.Contains("Host=;"))
{
    connectionString = configConnectionString;
    Console.WriteLine("[Config] Using configuration fallback.");
}

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}
else
{
    Console.WriteLine("[Config Warning] No database connection string found!");
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

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateFeedingEventCommand).Assembly));

// Application (MediatR + validators)
builder.Services.AddApplication();
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddScoped<GatewayAuthenticationService>();

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGateway", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Swagger
builder.Services.AddMicroserviceSwagger("Feeding Service API");


// Health checks UI
builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(30);
    opt.MaximumHistoryEntriesPerEndpoint(50);
})
.AddInMemoryStorage();

// Register Messenger
builder.Services.AddHttpClient();
builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feeding Service API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();

// app.UseHttpsRedirection(); // Disabled for internal service mesh

app.UseCors("AllowGateway");

app.UseAuthentication();
app.UseAuthorization();

// Apply automatic migrations on startup
app.ApplyMigrations<FeedingDbContext>();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecksUI(options => options.UIPath = "/health-ui");

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "FeedingService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();