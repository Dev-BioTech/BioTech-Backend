using SalesService.Application;
using SalesService.Infrastructure;
using SalesService.Infrastructure.Persistence;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Middlewares;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Standard Port Configuration for Railway/PaaS
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Configure Database Connection
// ------------------------------------------------------------------------------------------------
var configConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "";

string GetEnv(params string[] names) {
    foreach (var name in names) {
        var val = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrEmpty(val)) return val;
    }
    return null;
}

var dbHost = GetEnv("SALES_DB_HOST", "POSTGRESQL_ADDON_HOST", "DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = GetEnv("SALES_DB_PORT", "POSTGRESQL_ADDON_PORT", "DB_PORT") ?? "5432";
    var dbName = GetEnv("SALES_DB_NAME", "POSTGRESQL_ADDON_DB", "DB_DATABASE", "DB_NAME") ?? "biotech_db";
    var dbUser = GetEnv("SALES_DB_USER", "POSTGRESQL_ADDON_USER", "DB_USER");
    var dbPassword = GetEnv("SALES_DB_PASSWORD", "POSTGRESQL_ADDON_PASSWORD", "DB_PASSWORD");
    var dbSslMode = GetEnv("DB_SSL_MODE") ?? "Require";

    if (!string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
    {
        connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};Trust Server Certificate=true;";
    }
}

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
        }
        catch {}
    }
}

if (string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(configConnectionString))
{
    connectionString = configConnectionString;
}

if (!string.IsNullOrEmpty(connectionString))
{
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

// 2. JWT & Gateway Configuration
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
if (!string.IsNullOrEmpty(jwtSecret)) builder.Configuration["Jwt:Secret"] = jwtSecret;
var gatewaySecret = Environment.GetEnvironmentVariable("GATEWAY_SECRET");
if (!string.IsNullOrEmpty(gatewaySecret)) builder.Configuration["Gateway:Secret"] = gatewaySecret;

builder.Services.AddControllers();
builder.Services.AddMicroserviceSwagger("Sales Service API");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.ApplyMigrations<SalesDbContext>();

app.MapControllers();

app.MapGet("/version", () => new 
{ 
    Service = "SalesService", 
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();
