using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Commands;

public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, AnimalResponse>
{
    private readonly IAnimalRepository _animalRepository;

    public UpdateAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<AnimalResponse> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.Id, cancellationToken);
        if (animal == null)
            throw new KeyNotFoundException($"Animal with ID {request.Id} not found.");

        animal.UpdateDetails(
            request.VisualCode,
            request.ElectronicCode,
            request.Name,
            request.Color,
            request.BreedId,
            request.CategoryId,
            request.Purpose,
            request.Sex,
            request.BirthDate,
            request.Origin,
            request.InitialCost,
            request.EntryDate,
            request.MotherId,
            request.FatherId,
            request.ExternalMother,
            request.ExternalFather
        );

        await _animalRepository.UpdateAsync(animal, cancellationToken);

        return new AnimalResponse(
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
            animal.ExternalFather
        );
    }
}
