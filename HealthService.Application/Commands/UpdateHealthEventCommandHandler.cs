using MediatR;
using HealthService.Application.DTOs;
using HealthService.Domain.Entities;
using HealthService.Application.Interfaces;

namespace HealthService.Application.Commands;

public class UpdateHealthEventCommandHandler : IRequestHandler<UpdateHealthEventCommand, HealthEventResponse>
{
    private readonly IHealthEventRepository _healthEventRepository;

    public UpdateHealthEventCommandHandler(IHealthEventRepository healthEventRepository)
    {
        _healthEventRepository = healthEventRepository;
    }

    public async Task<HealthEventResponse> Handle(UpdateHealthEventCommand request, CancellationToken cancellationToken)
    {
        var healthEvent = await _healthEventRepository.GetByIdAsync(request.Id, cancellationToken);
        if (healthEvent == null)
        {
            throw new ArgumentException($"Health event with ID {request.Id} not found");
        }

        // Update properties
        if (request.Dto.Description != null)
        {
            healthEvent.Notes = request.Dto.Description;
        }

        if (request.Dto.Date.HasValue)
        {
            healthEvent.EventDate = request.Dto.Date.Value;
        }

        if (request.Dto.Treatment != null)
        {
            healthEvent.Treatment = request.Dto.Treatment;
        }

        if (request.Dto.VeterinarianName != null)
        {
            healthEvent.VeterinarianName = request.Dto.VeterinarianName;
        }

        healthEvent.UpdatedAt = DateTime.UtcNow;

        await _healthEventRepository.UpdateAsync(healthEvent, cancellationToken);

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
