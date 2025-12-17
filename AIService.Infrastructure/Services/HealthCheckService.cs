using System.Diagnostics;
using AIService.Application.DTOs;
using AIService.Application.Interfaces;

namespace AIService.Infrastructure.Services;

public class HealthCheckService : IHealthCheckService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    // In a real scenario, these might come from configuration or service discovery
    private readonly Dictionary<string, string> _serviceUrls = new()
    {
        { "FeedingService", "http://feeding-api:8080/health" },
        { "HerdService", "http://herd-api:8080/health" },
        { "HealthEventService", "http://health-event-api:8080/health" },
        { "AuthService", "http://auth-api:8080/health" }
    };

    public HealthCheckService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ServiceHealthStatus> CheckServiceHealthAsync(
        string serviceName,
        CancellationToken ct = default)
    {
        if (!_serviceUrls.TryGetValue(serviceName, out var url))
        {
            // Fallback: try to construct URL broadly if not in map
            url = $"http://{serviceName.ToLower().Replace("service", "-api")}:8080/health";
        }
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            
            var response = await client.GetAsync(url, ct);
            stopwatch.Stop();
            
            return new ServiceHealthStatus(
                ServiceName: serviceName,
                IsHealthy: response.IsSuccessStatusCode,
                ResponseTimeMs: (int)stopwatch.ElapsedMilliseconds,
                ErrorMessage: response.IsSuccessStatusCode 
                    ? null 
                    : $"HTTP {(int)response.StatusCode}",
                LastChecked: DateTime.UtcNow
            );
        }
        catch (TaskCanceledException)
        {
            return new ServiceHealthStatus(
                ServiceName: serviceName,
                IsHealthy: false,
                ResponseTimeMs: (int)stopwatch.ElapsedMilliseconds,
                ErrorMessage: "Timeout (>5s)",
                LastChecked: DateTime.UtcNow
            );
        }
        catch (Exception ex)
        {
            return new ServiceHealthStatus(
                ServiceName: serviceName,
                IsHealthy: false,
                ResponseTimeMs: (int)stopwatch.ElapsedMilliseconds,
                ErrorMessage: ex.Message,
                LastChecked: DateTime.UtcNow
            );
        }
    }
}
