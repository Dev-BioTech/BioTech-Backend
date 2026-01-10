using Shared.Infrastructure.Extensions;
using DotNetEnv;
using FeedingService.Application;
using FeedingService.Application.Commands.CreateFeedingEvent;
using FeedingService.Infrastructure;
using FeedingService.Presentation.Middlewares;
using FeedingService.Presentation.Services;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure Port for Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(int.Parse(port));
});

// Configure Database Connection from Environment Variables (Railway)
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME"); // Support both naming
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

    var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

Env.Load();
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
builder.Services.AddGlobalCors("BioTechCorsPolicy");

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

app.UseCors("BioTechCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecksUI(options => options.UIPath = "/health-ui");

app.Run();