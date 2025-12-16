using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Commands.CreateFarm;

public class CreateFarmCommandHandler : IRequestHandler<CreateFarmCommand, FarmResponse>
{
    private readonly IFarmRepository _farmRepository;
    private readonly ILogger<CreateFarmCommandHandler> _logger;

    public CreateFarmCommandHandler(IFarmRepository farmRepository, ILogger<CreateFarmCommandHandler> logger)
    {
        _farmRepository = farmRepository;
        _logger = logger;
    }

    public async Task<FarmResponse> Handle(CreateFarmCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new farm: {Name} for User: {UserId}", request.Name, request.UserId);

        var farm = new Farm
        {
            Name = request.Name,
            Owner = request.Owner,
            Address = request.Address,
            GeographicLocation = request.GeographicLocation,
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        farm.Validate();

        // Transaction handling would be ideal here to ensure both Farm and UserFarmRole are created
        // but Repository pattern abstracts this. Ideally, AddAsync should handle this or we coordinate via a Service.
        // For now, I'll save Farm, then link UserFarmRole.
        
        var createdFarm = await _farmRepository.AddAsync(farm, request.UserId, cancellationToken);
        
        // Link Creator to Farm if UserId is provided
        // Logic moved to Repository to be atomic with SaveChanges

        _logger.LogInformation("Farm created successfully with ID: {Id}", createdFarm.Id);

        return new FarmResponse(
            createdFarm.Id,
            createdFarm.Name,
            createdFarm.Owner,
            createdFarm.Address,
            createdFarm.GeographicLocation,
            createdFarm.Active,
            createdFarm.CreatedAt
        );
    }
}
