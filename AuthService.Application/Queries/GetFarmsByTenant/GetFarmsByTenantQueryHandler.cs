using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Queries.GetFarmsByTenant;

public class GetFarmsByTenantQueryHandler : IRequestHandler<GetFarmsByTenantQuery, FarmListResponse>
{
    private readonly IFarmRepository _farmRepository;
    private readonly ILogger<GetFarmsByTenantQueryHandler> _logger;

    public GetFarmsByTenantQueryHandler(IFarmRepository farmRepository, ILogger<GetFarmsByTenantQueryHandler> logger)
    {
        _farmRepository = farmRepository;
        _logger = logger;
    }

    public async Task<FarmListResponse> Handle(GetFarmsByTenantQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting farms for TenantUser: {TenantUserId}. IncludeInactive: {IncludeInactive}", 
            request.TenantUserId, request.IncludeInactive);
        
        var farms = await _farmRepository.GetByTenantUserIdAsync(
            request.TenantUserId, 
            request.IncludeInactive, 
            cancellationToken
        );

        var farmList = farms.Select(farm => new FarmResponse(
            farm.Id,
            farm.Name,
            farm.Owner,
            farm.Address,
            farm.GeographicLocation,
            farm.Active,
            farm.CreatedAt
        )).ToList();

        return new FarmListResponse(farmList, farmList.Count);
    }
}
