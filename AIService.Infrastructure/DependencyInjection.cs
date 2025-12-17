using AIService.Infrastructure.Persistence;
using AIService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using AIService.Application.Interfaces;
using AIService.Infrastructure.Services;
using AIService.Infrastructure.ExternalApis;

namespace AIService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IGeminiService, GeminiService>();
        services.AddHttpClient<AnthropicApiClient>(); // Registers AnthropicApiClient as a typed client
        
        services.AddDbContext<DiagnosticDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDiagnosticSessionRepository, DiagnosticSessionRepository>();
        services.AddScoped<ILogAnalyzerService, LogAnalyzerService>();
        
        // HealthCheckService uses IHttpClientFactory, so we register it as Scoped, 
        // and ensure AddHttpClient is called (which it is by the typed client calls above, but good to be explicit/safe)
        services.AddScoped<IHealthCheckService, HealthCheckService>();
        
        services.AddScoped<IDockerInspectorService, DockerInspectorService>();
        services.AddScoped<IAiAnalysisService, AiAnalysisService>();
        
        // Register IMessenger for inter-service communication
        services.AddHttpClient<Shared.Infrastructure.Interfaces.IMessenger, Shared.Infrastructure.Services.HttpMessenger>();
        
        // Configure named HttpClients for microservices
        // Railway uses environment variables for service URLs
        var herdServiceUrl = configuration["ServiceUrls:HerdService"] ?? "http://herd-service:8080";
        services.AddHttpClient("HerdService", client =>
        {
            client.BaseAddress = new Uri(herdServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        var healthServiceUrl = configuration["ServiceUrls:HealthService"] ?? "http://health-service:8080";
        services.AddHttpClient("HealthService", client =>
        {
            client.BaseAddress = new Uri(healthServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        var feedingServiceUrl = configuration["ServiceUrls:FeedingService"] ?? "http://feeding-service:8080";
        services.AddHttpClient("FeedingService", client =>
        {
            client.BaseAddress = new Uri(feedingServiceUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        return services;
    }
}
