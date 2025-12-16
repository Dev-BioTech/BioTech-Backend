using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Queries;

public record GetAnimalsByFarmQuery(int FarmId) : IRequest<IEnumerable<AnimalDto>>;

public class GetAnimalsByFarmQueryHandler : IRequestHandler<GetAnimalsByFarmQuery, IEnumerable<AnimalDto>>
{
    private readonly IAnimalRepository _repository;

    public GetAnimalsByFarmQueryHandler(IAnimalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AnimalDto>> Handle(GetAnimalsByFarmQuery request, CancellationToken cancellationToken)
    {
        var animals = await _repository.GetAllAsync(request.FarmId, cancellationToken);

        return animals.Select(a => new AnimalDto
        {
            Id = a.Id,
            VisualCode = a.VisualCode,
            ElectronicCode = a.ElectronicCode,
            Name = a.Name,
            FarmId = a.FarmId,
            CategoryId = a.CategoryId,
            Sex = a.Sex,
            CurrentStatus = a.CurrentStatus,
            BreedId = a.BreedId,
            BirthDate = a.BirthDate,
            InitialCost = a.InitialCost
        });
    }
}
