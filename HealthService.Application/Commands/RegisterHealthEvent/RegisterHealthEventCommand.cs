using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Commands;

public record RegisterHealthEventCommand(
    int FarmId,
    long? AnimalId,
    int? BatchId,
    string EventType,
    DateOnly EventDate,
    string? Disease,
    string? Treatment,
    string? Medication,
    decimal? Dosage,
    string? DosageUnit,
    string? VeterinarianName,
    decimal? Cost,
    string? Notes,
    DateOnly? NextFollowUpDate,
    bool RequiresFollowUp,
    string? FollowUpNotes,
    int? UserId
) : IRequest<HealthEventResponse>;
