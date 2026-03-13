using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries.GetMovementTypes;

public record GetMovementTypesQuery() : IRequest<IEnumerable<MovementTypeResponse>>;
