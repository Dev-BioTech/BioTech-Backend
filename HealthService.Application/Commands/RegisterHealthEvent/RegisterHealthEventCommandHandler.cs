using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;
using HealthService.Domain.Entities;
using FluentValidation;

namespace HealthService.Application.Commands;

public class RegisterHealthEventCommandHandler : IRequestHandler<RegisterHealthEventCommand, HealthEventResponse>
{
    private readonly IHealthEventRepository _repository;
    private readonly IValidator<RegisterHealthEventCommand> _validator;

    public RegisterHealthEventCommandHandler(
        IHealthEventRepository repository,
        IValidator<RegisterHealthEventCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<HealthEventResponse> Handle(RegisterHealthEventCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var healthEvent = new HealthEvent
        {
            FarmId = request.FarmId,
            AnimalId = request.AnimalId,
            BatchId = request.BatchId,
            EventType = request.EventType,
            EventDate = request.EventDate,
            Disease = request.Disease,
            Treatment = request.Treatment,
            Medication = request.Medication,
            Dosage = request.Dosage,
            DosageUnit = request.DosageUnit,
            VeterinarianName = request.VeterinarianName,
            Cost = request.Cost,
            Notes = request.Notes,
            NextFollowUpDate = request.NextFollowUpDate,
            RequiresFollowUp = request.RequiresFollowUp,
            FollowUpNotes = request.FollowUpNotes,
            CreatedBy = request.UserId,
            CreatedAt = DateTime.UtcNow
        };
        
        // Domain validation
        healthEvent.Validate();

        await _repository.AddAsync(healthEvent, cancellationToken);
        
        return new HealthEventResponse(
            healthEvent.Id,
            healthEvent.FarmId,
            healthEvent.AnimalId,
            healthEvent.BatchId,
            healthEvent.EventType,
            healthEvent.EventDate,
            healthEvent.Disease,
            healthEvent.Treatment,
            healthEvent.Medication,
            healthEvent.Dosage,
            healthEvent.DosageUnit,
            healthEvent.VeterinarianName,
            healthEvent.Cost,
            healthEvent.Notes,
            healthEvent.NextFollowUpDate,
            healthEvent.RequiresFollowUp,
            healthEvent.FollowUpNotes,
            healthEvent.CreatedAt,
            healthEvent.UpdatedAt
        );
    }
}
