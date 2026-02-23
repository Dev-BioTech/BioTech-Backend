using DotNetEnv;
using FeedingService.Presentation.Services;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Shared.Infrastructure.Extensions;
using FeedingService.Infrastructure.Persistence;
using FeedingService.Application;
using FeedingService.Infrastructure;
using FeedingService.Presentation.Middlewares;
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
var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI") ?? 
                       Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                       Environment.GetEnvironmentVariable("DB_URL");

if (!string.IsNullOrEmpty(connectionString))
{
    // Case 1: URI with scheme (postgresql://...) - Common in PaaS
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
            
            // Build standard connection string
            connectionString = $"Host={host};Port={parsedPort};Database={path};Username={user};Password={pass};Ssl Mode=Require;Trust Server Certificate=true;";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Config Error] Failed to parse URI connection string: {ex.Message}");
            // Fallback: Use string manipulation if Uri parsing fails
            if (connectionString.Contains("@"))
            {
                 connectionString = connectionString.Replace("postgresql://", "Host=");
                 var userInfoSplit = connectionString.IndexOf('@');
                 var userPassPart = connectionString.Substring(5, userInfoSplit - 5);
                 var hostPortDbPart = connectionString.Substring(userInfoSplit + 1);

                 var userPass = userPassPart.Split(':');
                 var hostPortDb = hostPortDbPart.Split('/');
                 var hostPort = hostPortDb[0].Split(':');

                 var host = hostPort[0];
                 var dbPort = hostPort.Length > 1 ? hostPort[1] : "5432";
                 var dbName = hostPortDb[1];
                 var user = userPass[0];
                 var password = userPass[1];

                 connectionString = $"Host={host};Port={dbPort};Database={dbName};Username={user};Password={password};Ssl Mode=Require;Trust Server Certificate=true;";
            }
        }
    }
}
else
{
    // Case 2: Individual variables - Common in Local/Docker
    var dbHost = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_HOST") ?? Environment.GetEnvironmentVariable("DB_HOST");
    if (!string.IsNullOrEmpty(dbHost))
    {
        var dbPort = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PORT") ?? Environment.GetEnvironmentVariable("DB_PORT");
        if (string.IsNullOrEmpty(dbPort)) dbPort = "5432";
        
        var dbName = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME") ?? "biotech_db";
        var dbUser = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
        var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

        connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};Trust Server Certificate=true;";
    }
}

// Set the configuration
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Feeding Service API",
        Version = "v1",
        Description = @"API for managing feeding events in livestock management system.

⚠️ IMPORTANT: This microservice uses Gateway Authentication.
- Direct calls require X-Gateway-Secret header
- In production, all requests should come through the API Gateway
- The Gateway validates JWT and forwards user information via headers"
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

app.UseMiddleware<ExceptionHandlingMiddleware>();
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