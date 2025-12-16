namespace HerdService.Application.DTOs;

public record AnimalMovementResponse(
    int Id,
    long AnimalId,
    int MovementTypeId,
    string MovementTypeName,
    DateTime MovementDate,
    string? Observation
);
