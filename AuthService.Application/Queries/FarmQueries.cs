using System.Collections.Generic;
using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Queries;

public record GetFarmByIdQuery(int Id) : IRequest<FarmResponseDto?>;
public record GetFarmsByTenantIdQuery(int TenantId) : IRequest<IEnumerable<FarmResponseDto>>;
