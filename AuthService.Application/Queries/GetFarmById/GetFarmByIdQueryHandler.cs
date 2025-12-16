using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Queries.GetFarmById;

public class GetFarmByIdQueryHandler : IRequestHandler<GetFarmByIdQuery, FarmResponse?>
{
    private readonly IFarmRepository _farmRepository;
    private readonly ILogger<GetFarmByIdQueryHandler> _logger;

    public GetFarmByIdQueryHandler(IFarmRepository farmRepository, ILogger<GetFarmByIdQueryHandler> logger)
    {
        _farmRepository = farmRepository;
        _logger = logger;
    }

    public async Task<FarmResponse?> Handle(GetFarmByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting farm with ID: {Id}", request.Id);
        
        var farm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);

        if (farm == null)
            return null;

        return new FarmResponse(
            farm.Id,
            farm.Name,
            farm.Owner,
            farm.Address,
            farm.GeographicLocation,
            farm.Active,
            farm.CreatedAt
        );
    }
}
