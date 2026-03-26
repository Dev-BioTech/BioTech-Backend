using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Queries;

public class GetAnimalsByFarmQueryHandler : IRequestHandler<GetAnimalsByFarmQuery, IEnumerable<AnimalResponse>>
{
    private readonly IAnimalRepository _animalRepository;

    public GetAnimalsByFarmQueryHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<IEnumerable<AnimalResponse>> Handle(GetAnimalsByFarmQuery request, CancellationToken cancellationToken)
    {
        var animals = await _animalRepository.GetByFarmIdAsync(request.FarmId, request.Status, request.IncludeInactive, cancellationToken);

        return animals.Select(animal => new AnimalResponse(
            animal.Id,
            animal.VisualCode,
            animal.ElectronicCode,
            animal.Name,
            animal.Color,
            animal.FarmId,
            animal.BreedId,
            animal.Breed?.Name,
            animal.CategoryId,
            animal.Category?.Name,
            animal.BatchId,
            animal.Batch?.Name,
            animal.PaddockId,
            animal.Paddock?.Name,
            animal.BirthDate,
            animal.GetAgeInMonths(),
            animal.Sex,
            animal.CurrentStatus,
            animal.Purpose,
            animal.Origin,
            animal.EntryDate,
            animal.InitialCost,
            animal.CurrentStatus == "ACTIVE",
            animal.MotherId,
            animal.FatherId,
            animal.ExternalMother,
            animal.ExternalFather,
            animal.Weight,
            animal.Height
        ));
    }
}
