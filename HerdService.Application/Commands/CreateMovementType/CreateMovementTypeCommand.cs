using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.CreateMovementType;

public record CreateMovementTypeCommand(string Name) : IRequest<MovementTypeResponse>;
