using DotNetEnv;
using HealthService.Application;
using HealthService.Infrastructure;
using HealthService.Presentation.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Port for Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Configure Database Connection from Environment Variables
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME") ?? "biotech_db";
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

    var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

Env.Load();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// Add Application & Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GatewayAuthenticationMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();