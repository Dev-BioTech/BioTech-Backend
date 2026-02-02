using DotNetEnv;
using HerdService.Application;
using HerdService.Infrastructure;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using HerdService.Presentation.Middlewares;
using Microsoft.OpenApi.Models;


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

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();

app.UseCors("AllowGateway");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HerdDbContext>();
    dbContext.Database.EnsureCreated();
}

// Version Endpoint
app.MapGet("/version", () => new 
{ 
    Service = "HerdService", 
    Version = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();
