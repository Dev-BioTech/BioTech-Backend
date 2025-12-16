using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Commands.CancelReproductionEvent;

public class CancelReproductionEventCommandHandler : IRequestHandler<CancelReproductionEventCommand, ReproductionEventResponse>
{
    private readonly IReproductionEventRepository _repository;

    public CancelReproductionEventCommandHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventResponse> Handle(CancelReproductionEventCommand request, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(request.Id, ct);

        if (entity == null)
            throw new KeyNotFoundException($"Reproduction event with id {request.Id} not found");

        entity.Cancel();

        await _repository.UpdateAsync(entity, ct);

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
