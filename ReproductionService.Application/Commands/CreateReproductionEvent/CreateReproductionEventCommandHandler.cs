using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;
using ReproductionService.Domain.ValueObjects;

namespace ReproductionService.Application.Commands.CreateReproductionEvent;

public class CreateReproductionEventCommandHandler : IRequestHandler<CreateReproductionEventCommand, ReproductionEventResponse>
{
    private readonly IReproductionEventRepository _repository;

    public CreateReproductionEventCommandHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventResponse> Handle(CreateReproductionEventCommand request, CancellationToken cancellationToken)
    {
        var reproductionEvent = new ReproductionEvent
        {
            FarmId = request.FarmId,
            EventDate = request.EventDate,
            AnimalId = request.AnimalId,
            EventType = request.EventType,
            Observations = request.Observations,
            Cost = request.Cost.HasValue ? Money.FromDecimal(request.Cost.Value) : Money.Zero(),
            SireId = request.SireId,
            IsPregnant = request.IsPregnant,
            OffspringCount = request.OffspringCount,
            RegisteredBy = request.RegisteredBy
        };

        reproductionEvent.Validate();

        var createdEvent = await _repository.AddAsync(reproductionEvent, cancellationToken);

        return new ReproductionEventResponse(
            createdEvent.Id,
            createdEvent.FarmId,
            createdEvent.EventDate,
            createdEvent.AnimalId,
            createdEvent.EventType,
            createdEvent.Observations,
            createdEvent.Cost?.Amount,
            createdEvent.SireId,
            createdEvent.IsPregnant,
            createdEvent.OffspringCount,
            createdEvent.CreatedAt,
            createdEvent.CreatedBy
        );
    }
}
