using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetHealthDashboardStatsQueryHandler : IRequestHandler<GetHealthDashboardStatsQuery, HealthDashboardStats>
{
    private readonly IHealthEventRepository _repository;

    public GetHealthDashboardStatsQueryHandler(IHealthEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<HealthDashboardStats> Handle(GetHealthDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var totalEvents = await _repository.GetTotalEventsCountAsync(request.FarmId, cancellationToken);
        var totalCost = await _repository.GetTotalCostAsync(request.FarmId, cancellationToken);
        var sickAnimals = await _repository.GetSickAnimalsCountAsync(request.FarmId, cancellationToken);

        return new HealthDashboardStats(
            totalEvents,
            totalCost,
            sickAnimals,
            DateTime.UtcNow
        );
    }
}
