using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetUpcomingHealthEventsQueryHandler : IRequestHandler<GetUpcomingHealthEventsQuery, IEnumerable<HealthEventResponse>>
{
    private readonly IHealthEventRepository _repository;

    public GetUpcomingHealthEventsQueryHandler(IHealthEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<HealthEventResponse>> Handle(GetUpcomingHealthEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetUpcomingEventsAsync(request.FarmId, request.Limit, cancellationToken);

        return events.Select(e => new HealthEventResponse(
            e.Id,
            e.FarmId,
            e.AnimalId,
            e.BatchId,
            e.EventType,
            e.EventDate,
            e.Disease,
            e.Treatment,
            e.Medication,
            e.Dosage,
            e.DosageUnit,
            e.VeterinarianName,
            e.Cost,
            e.Notes,
            e.NextFollowUpDate,
            e.RequiresFollowUp,
            e.FollowUpNotes,
            e.CreatedAt,
            e.UpdatedAt
        ));
    }
}
