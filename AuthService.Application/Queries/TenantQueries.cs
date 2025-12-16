using System.Collections.Generic;
using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Queries;

public record GetTenantByIdQuery(int Id) : IRequest<TenantResponseDto?>;
public record GetAllTenantsQuery() : IRequest<IEnumerable<TenantResponseDto>>;
