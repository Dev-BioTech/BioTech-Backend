using MediatR;
using HealthService.Application.DTOs;
using HealthService.Domain.Entities;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetUpcomingHealthEventsQueryHandler : IRequestHandler<GetUpcomingHealthEventsQuery, IEnumerable<UpcomingHealthEventDto>>
{
    private readonly IHealthEventRepository _healthEventRepository;

    public GetUpcomingHealthEventsQueryHandler(IHealthEventRepository healthEventRepository)
    {
        _healthEventRepository = healthEventRepository;
    }

    public async Task<IEnumerable<UpcomingHealthEventDto>> Handle(GetUpcomingHealthEventsQuery request, CancellationToken cancellationToken)
    {
        var limit = Math.Min(request.Limit, 10); // Max 10 records

        var upcomingEvents = await _healthEventRepository.GetUpcomingEventsAsync(request.FarmId, limit, cancellationToken);
        
        return upcomingEvents.Select(healthEvent => new UpcomingHealthEventDto(
            healthEvent.Id,
            $"Animal-{healthEvent.AnimalId}", // Placeholder - would come from AnimalRepository in real implementation
            healthEvent.EventType,
            healthEvent.EventDate
        ));
    }
}
