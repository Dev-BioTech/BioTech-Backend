using MediatR;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CommercialService.Application.Queries;

// List
public record GetThirdPartiesQuery(int FarmId, bool? IsSupplier, bool? IsCustomer) : IRequest<List<ThirdPartyDto>>;

public class GetThirdPartiesQueryHandler : IRequestHandler<GetThirdPartiesQuery, List<ThirdPartyDto>>
{
    private readonly IThirdPartyRepository _repository;

    public GetThirdPartiesQueryHandler(IThirdPartyRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ThirdPartyDto>> Handle(GetThirdPartiesQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetAllAsync(request.FarmId, request.IsSupplier, request.IsCustomer, cancellationToken);

        return list.Select(tp => new ThirdPartyDto
        {
            Id = tp.Id,
            FarmId = tp.FarmId,
            FullName = tp.FullName,
            IdentityDocument = tp.IdentityDocument,
            Phone = tp.Phone,
            Email = tp.Email,
            IsSupplier = tp.IsSupplier,
            IsCustomer = tp.IsCustomer,
            IsEmployee = tp.IsEmployee,
            IsVeterinarian = tp.IsVeterinarian,
            Address = tp.Address
        }).ToList();
    }
}

// By Id
public record GetThirdPartyByIdQuery(long Id) : IRequest<ThirdPartyDto?>;

public class GetThirdPartyByIdQueryHandler : IRequestHandler<GetThirdPartyByIdQuery, ThirdPartyDto?>
{
    private readonly IThirdPartyRepository _repository;

    public GetThirdPartyByIdQueryHandler(IThirdPartyRepository repository)
    {
        _repository = repository;
    }

    public async Task<ThirdPartyDto?> Handle(GetThirdPartyByIdQuery request, CancellationToken cancellationToken)
    {
        var tp = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tp == null) return null;

        return new ThirdPartyDto
        {
            Id = tp.Id,
            FarmId = tp.FarmId,
            FullName = tp.FullName,
            IdentityDocument = tp.IdentityDocument,
            Phone = tp.Phone,
            Email = tp.Email,
            IsSupplier = tp.IsSupplier,
            IsCustomer = tp.IsCustomer,
            IsEmployee = tp.IsEmployee,
            IsVeterinarian = tp.IsVeterinarian,
            Address = tp.Address
        };
    }
}
