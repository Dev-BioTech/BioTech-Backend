using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Queries.GetFarmById;

public record GetFarmByIdQuery(int Id) : IRequest<FarmResponse?>;
