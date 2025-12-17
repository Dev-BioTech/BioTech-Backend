using AIService.Application;
using AIService.Infrastructure;
using AIService.Presentation.Middlewares;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AIService.Presentation.Services.GatewayAuthenticationService>();

builder.Services.AddHttpClient("HerdService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("HERD_SERVICE_URL") 
        ?? "http://herd-service:8080");
});

builder.Services.AddHttpClient("HealthService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("HEALTH_SERVICE_URL") 
        ?? "http://health-service:8080");
});

builder.Services.AddHttpClient("FeedingService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FEEDING_SERVICE_URL") 
        ?? "http://feeding-service:8080");
});

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "your-secret-key";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "BioTech";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "BioTech";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI Service API",
        Version = "v1",
        Description = @"API for AI-powered chat assistance in livestock management system.

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AI Service API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<GatewayAuthenticationMiddleware>();

app.UseCors("AllowGateway");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

