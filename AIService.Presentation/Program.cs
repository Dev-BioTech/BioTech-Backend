using AIService.Application;
using AIService.Infrastructure;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
