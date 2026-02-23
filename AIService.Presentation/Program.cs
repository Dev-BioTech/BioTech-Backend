using AIService.Application;
using AIService.Infrastructure;
using DotNetEnv;
using Shared.Infrastructure.Extensions;
using AIService.Infrastructure.Persistence;
using Shared.Infrastructure.Middlewares;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Layer dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure named clients for inter-service communication
builder.Services.AddHttpClient();

var herdServiceUrl = Environment.GetEnvironmentVariable("HERD_SERVICE_URL") ?? "http://localhost:5048";
builder.Services.AddHttpClient("HerdService", client => { client.BaseAddress = new Uri(herdServiceUrl); });

var healthServiceUrl = Environment.GetEnvironmentVariable("HEALTH_SERVICE_URL") ?? "http://localhost:5158";
builder.Services.AddHttpClient("HealthService", client => { client.BaseAddress = new Uri(healthServiceUrl); });

var feedingServiceUrl = Environment.GetEnvironmentVariable("FEEDING_SERVICE_URL") ?? "http://localhost:5102";
builder.Services.AddHttpClient("FeedingService", client => { client.BaseAddress = new Uri(feedingServiceUrl); });

builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();

// Register Gateway Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AIService.Presentation.Services.GatewayAuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Often disabled in internal microservices behind gateway

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthorization();

// Apply automatic migrations on startup
app.ApplyMigrations<DiagnosticDbContext>();

app.MapControllers();

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "AIService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();
