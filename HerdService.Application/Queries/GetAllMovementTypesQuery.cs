using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Queries;

public record GetAllMovementTypesQuery : IRequest<IEnumerable<MovementTypeResponse>>;
