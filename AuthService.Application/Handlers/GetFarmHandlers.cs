using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Application.Queries;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Handlers;

public class GetFarmByIdHandler : IRequestHandler<GetFarmByIdQuery, FarmResponseDto?>
{
    private readonly IFarmRepository _farmRepository;

    public GetFarmByIdHandler(IFarmRepository farmRepository)
    {
        _farmRepository = farmRepository;
    }

    public async Task<FarmResponseDto?> Handle(GetFarmByIdQuery request, CancellationToken cancellationToken)
    {
        var farm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);
        if (farm == null) return null;

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

public class GetFarmsByTenantIdHandler : IRequestHandler<GetFarmsByTenantIdQuery, IEnumerable<FarmResponseDto>>
{
    private readonly IFarmRepository _farmRepository;

    public GetFarmsByTenantIdHandler(IFarmRepository farmRepository)
    {
        _farmRepository = farmRepository;
    }

    public async Task<IEnumerable<FarmResponseDto>> Handle(GetFarmsByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var farms = await _farmRepository.GetAllByTenantIdAsync(request.TenantId, cancellationToken);

        return farms.Select(f => new FarmResponseDto
        {
            Id = f.Id,
            Name = f.Name,
            TenantId = f.TenantId,
            Address = f.Address,
            Location = f.Location,
            CreatedAt = f.CreatedAt
        });
    }
}
