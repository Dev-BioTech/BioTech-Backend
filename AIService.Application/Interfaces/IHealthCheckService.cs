using AIService.Application.DTOs;

namespace AIService.Application.Interfaces;

public interface IHealthCheckService
{
    Task<ServiceHealthStatus> CheckServiceHealthAsync(string serviceName, CancellationToken ct = default);
}
