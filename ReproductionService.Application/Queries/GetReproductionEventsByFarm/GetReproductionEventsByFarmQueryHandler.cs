using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries.GetReproductionEventsByFarm;

public class GetReproductionEventsByFarmQueryHandler : IRequestHandler<GetReproductionEventsByFarmQuery, ReproductionEventListResponse>
{
    private readonly IReproductionEventRepository _repository;

    public GetReproductionEventsByFarmQueryHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventListResponse> Handle(GetReproductionEventsByFarmQuery request, CancellationToken ct)
    {
        var events = await _repository.GetByFarmIdAsync(request.FarmId, request.FromDate, request.ToDate, ct);
        var responses = events.Select(MapToResponse).ToList();
        return new ReproductionEventListResponse(responses, responses.Count);
    }

    private static ReproductionEventResponse MapToResponse(ReproductionEvent entity)
    {
        return new ReproductionEventResponse(
            entity.Id,
            entity.FarmId,
            entity.EventDate,
            entity.AnimalId,
            entity.EventType,
            entity.Observations,
            entity.MaleAnimalId,
            entity.SemenBatchId,
            entity.PregnancyResult,
            entity.OffspringCount,
            entity.IsCancelled,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.RegisteredBy
        );
    }
}
