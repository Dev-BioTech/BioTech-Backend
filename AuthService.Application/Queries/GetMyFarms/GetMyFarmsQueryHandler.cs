using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using MediatR;

namespace AuthService.Application.Queries.GetMyFarms;

public class GetMyFarmsQueryHandler : IRequestHandler<GetMyFarmsQuery, FarmListResponse>
{
    private readonly IFarmRepository _farmRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyFarmsQueryHandler(IFarmRepository farmRepository, ICurrentUserService currentUserService)
    {
        _farmRepository = farmRepository;
        _currentUserService = currentUserService;
    }

    public async Task<FarmListResponse> Handle(GetMyFarmsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var farms = await _farmRepository.GetByTenantUserIdAsync(userId.Value, request.IncludeInactive, cancellationToken);
        
        var farmResponses = farms.Select(farm => new FarmResponse(
            farm.Id,
            farm.Name,
            farm.Owner,
            farm.Address,
            farm.GeographicLocation,
            farm.Active,
            farm.CreatedAt
        )).ToList();

        return new FarmListResponse(farmResponses, farmResponses.Count);
    }
}
