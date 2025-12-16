using HealthService.Domain.ValueObjects;

namespace HealthService.Application.DTOs;

public record HealthEventResponse(
    long Id,
    int FarmId,
    DateTime EventDate,
    string EventType,
    int? BatchId,
    long? AnimalId,
    int? DiseaseDiagnosisId,
    long? ProfessionalId,
    decimal ServiceCost,
    string? Observations,
    DateTime CreatedAt,
    int? CreatedBy
);
