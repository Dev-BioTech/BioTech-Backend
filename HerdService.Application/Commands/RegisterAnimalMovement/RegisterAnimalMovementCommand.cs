using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.RegisterAnimalMovement;

public record RegisterAnimalMovementCommand(
    long AnimalId,
    int MovementTypeId,
    DateTime MovementDate,
    string? Observation
) : IRequest<AnimalMovementResponse>;
