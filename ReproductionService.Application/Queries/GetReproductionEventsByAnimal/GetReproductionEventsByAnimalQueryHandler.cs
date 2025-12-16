using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries.GetReproductionEventsByAnimal;

public class GetReproductionEventsByAnimalQueryHandler : IRequestHandler<GetReproductionEventsByAnimalQuery, ReproductionEventListResponse>
{
    private readonly IReproductionEventRepository _repository;

    public GetReproductionEventsByAnimalQueryHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventListResponse> Handle(GetReproductionEventsByAnimalQuery request, CancellationToken ct)
    {
        var events = await _repository.GetByAnimalIdAsync(request.AnimalId, ct);
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
