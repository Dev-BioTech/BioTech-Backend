using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Queries.GetFarmsByTenant;

public record GetFarmsByTenantQuery(
    int TenantUserId,
    bool IncludeInactive = false
) : IRequest<FarmListResponse>;
