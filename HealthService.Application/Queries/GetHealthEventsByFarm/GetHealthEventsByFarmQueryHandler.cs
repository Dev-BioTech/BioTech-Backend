using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Queries;

public class GetHealthEventsByFarmQueryHandler : IRequestHandler<GetHealthEventsByFarmQuery, HealthEventListResponse>
{
    private readonly IHealthEventRepository _repository;

    public GetHealthEventsByFarmQueryHandler(IHealthEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<HealthEventListResponse> Handle(GetHealthEventsByFarmQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetByFarmIdAsync(
            request.FarmId, 
            request.FromDate, 
            request.ToDate, 
            request.EventType, 
            cancellationToken);

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
