using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Commands.CreateReproductionEvent;

public class CreateReproductionEventCommandHandler : IRequestHandler<CreateReproductionEventCommand, ReproductionEventResponse>
{
    private readonly IReproductionEventRepository _repository;

    public CreateReproductionEventCommandHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventResponse> Handle(CreateReproductionEventCommand request, CancellationToken ct)
    {
        var entity = new ReproductionEvent(
            request.FarmId,
            request.AnimalId,
            request.EventType,
            request.EventDate,
            request.RegisteredBy,
            request.Observations
        )
        {
            MaleAnimalId = request.MaleAnimalId,
            SemenBatchId = request.SemenBatchId,
            PregnancyResult = request.PregnancyResult,
            OffspringCount = request.OffspringCount
        };

        entity.Validate();

        var created = await _repository.AddAsync(entity, ct);

        return MapToResponse(created);
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
