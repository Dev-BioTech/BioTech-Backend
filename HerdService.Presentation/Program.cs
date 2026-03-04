using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using HerdService.Presentation.Middlewares;
using Microsoft.OpenApi.Models;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Middlewares;
using HerdService.Application;
using HerdService.Infrastructure;
using HerdService.Infrastructure.Persistence;


Env.TraversePath().Load(); // Moved to top

// Enable legacy timestamp behavior to handle DateTime Kind (UTC/Unspecified) issues
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


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
var dbHost = GetEnv("HERD_DB_HOST", "POSTGRESQL_ADDON_HOST", "DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = GetEnv("HERD_DB_PORT", "POSTGRESQL_ADDON_PORT", "DB_PORT") ?? "5432";
    var dbName = GetEnv("HERD_DB_NAME", "POSTGRESQL_ADDON_DB", "DB_DATABASE", "DB_NAME") ?? "biotech_db";
    var dbUser = GetEnv("HERD_DB_USER", "POSTGRESQL_ADDON_USER", "DB_USER");
    var dbPassword = GetEnv("HERD_DB_PASSWORD", "POSTGRESQL_ADDON_PASSWORD", "DB_PASSWORD");
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

builder.Services.AddControllers();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => { });

// Add Authorization
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

// Configure CORS
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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Herd Service API",
        Version = "v1",
        Description = "API for managing herd and animals."
    });

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

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddDbContextCheck<HerdDbContext>();

// Register Messenger
builder.Services.AddHttpClient();
builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();
builder.Services.AddScoped<HerdService.Application.Interfaces.IBatchRepository, HerdService.Infrastructure.Repositories.BatchRepository>();

// Register Gateway Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HerdService.Presentation.Services.GatewayAuthenticationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for internal service mesh

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();

app.UseCors("AllowGateway");

app.UseAuthentication();
app.UseAuthorization();

// Apply automatic migrations on startup
app.ApplyMigrations<HerdDbContext>();

app.MapControllers();

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "HerdService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();
