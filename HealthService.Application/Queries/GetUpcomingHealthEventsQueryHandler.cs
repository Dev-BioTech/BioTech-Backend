using MediatR;
using HealthService.Application.DTOs;
using HealthService.Domain.Entities;
using HealthService.Application.Interfaces;
using AuthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetUpcomingHealthEventsQueryHandler : IRequestHandler<GetUpcomingHealthEventsQuery, IEnumerable<UpcomingHealthEventDto>>
{
    private readonly IHealthEventRepository _healthEventRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUpcomingHealthEventsQueryHandler(IHealthEventRepository healthEventRepository, ICurrentUserService currentUserService)
    {
        _healthEventRepository = healthEventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<UpcomingHealthEventDto>> Handle(GetUpcomingHealthEventsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var limit = Math.Min(request.Limit, 10); // Max 10 records

        var upcomingEvents = await _healthEventRepository.GetUpcomingEventsAsync(userId.Value, today, limit, cancellationToken);
        
        return upcomingEvents.Select(healthEvent => new UpcomingHealthEventDto(
            healthEvent.Id,
            $"Animal-{healthEvent.AnimalId}", // Placeholder - would come from AnimalRepository in real implementation
            healthEvent.EventType,
            healthEvent.EventDate
        ));
    }
}
