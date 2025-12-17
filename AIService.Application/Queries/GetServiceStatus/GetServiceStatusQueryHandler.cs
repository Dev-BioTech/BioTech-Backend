using AIService.Application.DTOs;
using AIService.Application.Interfaces;
using MediatR;

namespace AIService.Application.Queries.GetServiceStatus;

public class GetServiceStatusQueryHandler : IRequestHandler<GetServiceStatusQuery, ServiceStatusResponse>
{
    private readonly IHealthCheckService _healthCheckService;
    // List of known services to check
    private readonly List<string> _knownServices = new()
    {
        "FeedingService", "HerdService", "HealthEventService", "AuthService", "InventoryService", "CommercialService", "ReproductionService"
    };

    public GetServiceStatusQueryHandler(IHealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

    public async Task<ServiceStatusResponse> Handle(GetServiceStatusQuery request, CancellationToken cancellationToken)
    {
        var tasks = _knownServices.Select(s => _healthCheckService.CheckServiceHealthAsync(s, cancellationToken));
        var results = await Task.WhenAll(tasks);
        
        var servicesMap = results.ToDictionary(r => r.ServiceName);
        
        return new ServiceStatusResponse(servicesMap, DateTime.UtcNow);
    }
}
