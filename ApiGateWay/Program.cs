using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Port for Railway
// Railway provides the PORT environment variable. We use this to tell Kestrel where to listen.
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// 2. Add Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configure CORS (Merged User Request)
builder.Services.AddGlobalCors("BioTechCorsPolicy");

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

// 5. Database Context (Required if Gateway accesses DB directly)
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"Ssl Mode={Environment.GetEnvironmentVariable("DB_SSL_MODE")};";

// If you have a DbContext, ensure it is registered here, e.g.:
// builder.Services.AddDbContext<ApiGateWay.Data.AuthDbContext>(options => options.UseNpgsql(connectionString));

// 6. JWT Authentication
var secretKey = builder.Configuration["JwtConfig:Secret"];
if (string.IsNullOrEmpty(secretKey)) secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");

var issuer = builder.Configuration["JwtConfig:Issuer"];
if (string.IsNullOrEmpty(issuer)) issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

var audience = builder.Configuration["JwtConfig:Audience"];
if (string.IsNullOrEmpty(audience)) audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

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
app.UseCors("BioTechCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 9. Use Ocelot
app.UseOcelot().Wait();

app.Run();