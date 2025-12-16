using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries.GetReproductionEventById;

public class GetReproductionEventByIdQueryHandler : IRequestHandler<GetReproductionEventByIdQuery, ReproductionEventResponse>
{
    private readonly IReproductionEventRepository _repository;

    public GetReproductionEventByIdQueryHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventResponse> Handle(GetReproductionEventByIdQuery request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id, ct);

        if (entity == null)
            return null;

        return MapToResponse(entity);
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
