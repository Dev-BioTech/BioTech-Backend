using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetHealthEventsByBatchQueryHandler : IRequestHandler<GetHealthEventsByBatchQuery, HealthEventListResponse>
{
    private readonly IHealthEventRepository _repository;

    public GetHealthEventsByBatchQueryHandler(IHealthEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<HealthEventListResponse> Handle(GetHealthEventsByBatchQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetByBatchIdAsync(request.BatchId, request.EventType, cancellationToken);

        var responseList = events.Select(e => new HealthEventResponse(
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

        return new HealthEventListResponse(responseList, events.Count());
    }
}
