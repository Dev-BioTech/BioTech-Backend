using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Commands.CreateHealthEvent;

public record CreateHealthEventCommand(
    int FarmId,
    DateTime EventDate,
    string EventType,
    int? BatchId,
    long? AnimalId,
    int? DiseaseDiagnosisId,
    long? ProfessionalId,
    decimal ServiceCost,
    string? Observations,
    int? RegisteredBy // User ID
) : IRequest<HealthEventResponse>;
