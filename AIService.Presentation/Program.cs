using AIService.Application;
using AIService.Infrastructure;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Add services to the container.
// Configure Port for Railway / Cloud (Only if PORT is set)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(int.Parse(port));
    });
}

// Configure Gateway Secret from Environment Variables
var gatewaySecret = Environment.GetEnvironmentVariable("GATEWAY_SECRET");
if (!string.IsNullOrEmpty(gatewaySecret))
{
    builder.Configuration["Gateway:Secret"] = gatewaySecret;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AIService", Version = "v1" });

    c.AddSecurityDefinition("Gateway", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Gateway Secret for direct access (X-Gateway-Secret header)",
        Name = "X-Gateway-Secret",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Gateway"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Gateway"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Layer dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register Gateway Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AIService.Presentation.Services.GatewayAuthenticationService>();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AIService.Presentation.Middlewares.GatewayAuthenticationMiddleware>();

// app.UseHttpsRedirection(); // Often disabled in internal microservices behind gateway

app.UseAuthorization();

app.MapControllers();

app.Run();
