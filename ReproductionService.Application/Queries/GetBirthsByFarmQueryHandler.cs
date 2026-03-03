using MediatR;
using ReproductionService.Application.Interfaces;
using ReproductionService.Application.DTOs;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries;

public class GetBirthsByFarmQueryHandler : IRequestHandler<GetBirthsByFarmQuery, IEnumerable<BirthDto>>
{
    private readonly IBirthRepository _birthRepository;

    public GetBirthsByFarmQueryHandler(IBirthRepository birthRepository)
    {
        _birthRepository = birthRepository;
    }

    public async Task<IEnumerable<BirthDto>> Handle(GetBirthsByFarmQuery request, CancellationToken cancellationToken)
    {
        var births = await _birthRepository.GetByFarmIdAsync(request.FarmId, cancellationToken);
        
        return births.Select(birth => new BirthDto(
            birth.MotherAnimalId,
            birth.OffspringTag,
            birth.BirthDate,
            birth.Weight,
            birth.Gender
        ));
    }
}
