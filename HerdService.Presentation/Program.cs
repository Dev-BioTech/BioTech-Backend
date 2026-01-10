using Shared.Infrastructure.Extensions;
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
    var dbName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var dbSslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

    var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};Ssl Mode={dbSslMode};";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}



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
builder.Services.AddGlobalCors("BioTechCorsPolicy");

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

app.UseCors("BioTechCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HerdDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
