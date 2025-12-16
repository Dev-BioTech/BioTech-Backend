using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Handlers;

public class CreateFarmHandler : IRequestHandler<CreateFarmCommand, FarmResponseDto>
{
    private readonly IFarmRepository _farmRepository;
    private readonly ITenantRepository _tenantRepository;

    public CreateFarmHandler(IFarmRepository farmRepository, ITenantRepository tenantRepository)
    {
        _farmRepository = farmRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task<FarmResponseDto> Handle(CreateFarmCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Tenant exists
        var tenantExists = await _tenantRepository.ExistsAsync(request.FarmDto.TenantId, cancellationToken);
        if (!tenantExists)
        {
            throw new KeyNotFoundException($"Tenant with ID {request.FarmDto.TenantId} not found.");
        }

        // 2. Create Entity
        var farm = new Farm
        {
            Name = request.FarmDto.Name,
            TenantId = request.FarmDto.TenantId,
            Address = request.FarmDto.Address,
            Location = request.FarmDto.Location,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // 3. Save
        await _farmRepository.AddAsync(farm, cancellationToken);

        // 4. Return DTO
        return new FarmResponseDto
        {
            Id = farm.Id,
            Name = farm.Name,
            TenantId = farm.TenantId,
            Address = farm.Address,
            Location = farm.Location,
            CreatedAt = farm.CreatedAt
        };
    }
}
