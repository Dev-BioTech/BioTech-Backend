using AuthService.Application;
using AuthService.Infrastructure;
using DotNetEnv;
using Shared.Infrastructure.Extensions;
using AuthService.Infrastructure.Persistence;
using Shared.Infrastructure.Middlewares;
using AuthService.Presentation.Middlewares;

// Enable legacy timestamp behavior to handle DateTime Kind (UTC/Unspecified) issues
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddMicroserviceSwagger("AuthService");

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
var configConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "";

// Helper to get non-empty environment variable
string? GetEnv(params string[] names) {
    foreach (var name in names) {
        var val = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrEmpty(val)) return val;
    }
    return null;
}

// Priority 1: Individual variables (Ensures port overrides like 50013 are used)
var dbHost = GetEnv("AUTH_DB_HOST", "POSTGRESQL_ADDON_HOST", "DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = GetEnv("AUTH_DB_PORT", "POSTGRESQL_ADDON_PORT", "DB_PORT") ?? "5432";
    var dbName = GetEnv("AUTH_DB_NAME", "POSTGRESQL_ADDON_DB", "DB_DATABASE", "DB_NAME") ?? "biotech_db";
    var dbUser = GetEnv("AUTH_DB_USER", "POSTGRESQL_ADDON_USER", "DB_USER");
    var dbPassword = GetEnv("AUTH_DB_PASSWORD", "POSTGRESQL_ADDON_PASSWORD", "DB_PASSWORD");
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

// Register Messenger
builder.Services.AddHttpClient();
builder.Services.AddScoped<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();

// Register Presentation Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService.Presentation.Services.GatewayAuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for internal service mesh

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Apply automatic migrations on startup
app.ApplyMigrations<AuthDbContext>();

app.MapControllers();

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "AuthService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();