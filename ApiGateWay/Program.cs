using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Port for Railway
// Railway provides the PORT environment variable. We use this to tell Kestrel where to listen.
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(int.Parse(port));
    });
}

// 2. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configure CORS (Merged User Request)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVercel", policy =>
    {
        policy.SetIsOriginAllowed(origin => 
        {
            // Allow localhost for development
            if (origin.StartsWith("http://localhost")) return true;
            // Allow any Vercel subdomain
            if (origin.EndsWith(".vercel.app")) return true;
            // Allow specific production domains if added later
            return false;
        })
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// 4. Ocelot Configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Dynamic Ocelot Configuration for Railway
// Railway doesn't have internal networking, so we need to use public URLs
var routes = builder.Configuration.GetSection("Routes").GetChildren();
foreach (var route in routes)
{
    var downstreamHosts = route.GetSection("DownstreamHostAndPorts").GetChildren();
    foreach (var hostConfig in downstreamHosts)
    {
        var host = hostConfig.GetValue<string>("Host");
        
        // Check if we're running on Railway (environment variables will have full URLs)
        // Map service definition names to their environment variable prefixes
        var serviceEnvMap = new Dictionary<string, string>
        {
            { "auth-service", "AUTH_SERVICE" },
            { "ai-service", "AI_SERVICE" },
            { "feeding-service", "FEEDING_SERVICE" },
            { "herd-service", "HERD_SERVICE" },
            { "reproduction-service", "REPRODUCTION_SERVICE" },
            { "health-service", "HEALTH_SERVICE" },
            { "commercial-service", "COMMERCIAL_SERVICE" },
            { "inventory-service", "INVENTORY_SERVICE" }
        };

        if (!string.IsNullOrEmpty(host) && serviceEnvMap.TryGetValue(host, out var envPrefix))
        {
            var serviceUrl = Environment.GetEnvironmentVariable($"{envPrefix}_URL");
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                try
                {
                    // Parse the URL to extract host and port
                    var uri = new Uri(serviceUrl);
                    hostConfig["Host"] = uri.Host;
                    hostConfig["Port"] = uri.Port.ToString();
                    route["DownstreamScheme"] = uri.Scheme;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"WARNING: Invalid URI for {host} (Value: '{serviceUrl}'): {ex.Message}");
                    // Fallback to existing or empty values to prevent crash
                }
            }
            else
            {
                // Fallback to separate host/port env vars (for Docker Compose)
                var envHost = Environment.GetEnvironmentVariable($"{envPrefix}_HOST");
                var envPort = Environment.GetEnvironmentVariable($"{envPrefix}_PORT");
                if (!string.IsNullOrEmpty(envHost)) hostConfig["Host"] = envHost;
                if (!string.IsNullOrEmpty(envPort)) hostConfig["Port"] = envPort;
            }
        }
    }
}

// Register GatewayHeaderHandler
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ApiGateWay.Handlers.GatewayHeaderHandler>();

builder.Services.AddOcelot(builder.Configuration)
    .AddDelegatingHandler<ApiGateWay.Handlers.GatewayHeaderHandler>(true);

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

// Backward compatibility variables for existing code below
var secretKey = builder.Configuration["Jwt:Secret"] ?? builder.Configuration["JwtConfig:Secret"];
var issuer = builder.Configuration["Jwt:Issuer"] ?? builder.Configuration["JwtConfig:Issuer"];
var audience = builder.Configuration["Jwt:Audience"] ?? builder.Configuration["JwtConfig:Audience"];

if (!string.IsNullOrEmpty(secretKey) && secretKey.Length >= 16)
{
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
}

var app = builder.Build();

// 7. Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Not needed on Railway, handled at edge

// Ensure Routing is called before CORS
app.UseRouting();

// 8. Use CORS (Must be before Auth)
app.UseCors("AllowVercel");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 9. Use Ocelot
app.UseOcelot().Wait();

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "ApiGateWay", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();