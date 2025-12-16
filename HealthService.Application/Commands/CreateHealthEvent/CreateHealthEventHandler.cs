using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;
using HealthService.Application.Events;
using HealthService.Domain.Entities;
using HealthService.Domain.ValueObjects;

namespace HealthService.Application.Commands.CreateHealthEvent;

public class CreateHealthEventHandler : IRequestHandler<CreateHealthEventCommand, HealthEventResponse>
{
    private readonly IHealthEventRepository _repository;
    private readonly IEventBus _eventBus;

    public CreateHealthEventHandler(IHealthEventRepository repository, IEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }

    public async Task<HealthEventResponse> Handle(CreateHealthEventCommand request, CancellationToken cancellationToken)
    {
        var healthEvent = new HealthEvent
        {
            FarmId = request.FarmId,
            EventDate = request.EventDate,
            EventType = request.EventType,
            BatchId = request.BatchId,
            AnimalId = request.AnimalId,
            DiseaseDiagnosisId = request.DiseaseDiagnosisId,
            ProfessionalId = request.ProfessionalId,
            ServiceCost = Money.FromDecimal(request.ServiceCost),
            Observations = request.Observations,
            CreatedBy = request.RegisteredBy
        };

        healthEvent.Validate();

        await _repository.AddAsync(healthEvent, cancellationToken);
        
        // Publish Event
        var eventRegistered = new HealthEventRegistered(
            healthEvent.Id, 
            healthEvent.FarmId, 
            healthEvent.EventType, 
            healthEvent.EventDate
        );
        
        await _eventBus.PublishAsync(eventRegistered, cancellationToken);

        return new HealthEventResponse(
            healthEvent.Id,
            healthEvent.FarmId,
            healthEvent.EventDate,
            healthEvent.EventType,
            healthEvent.BatchId,
            healthEvent.AnimalId,
            healthEvent.DiseaseDiagnosisId,
            healthEvent.ProfessionalId,
            healthEvent.ServiceCost.Amount,
            healthEvent.Observations,
            healthEvent.CreatedAt,
            healthEvent.CreatedBy
        );
    }
}
