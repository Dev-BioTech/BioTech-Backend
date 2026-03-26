using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Commands;

public class SellAnimalCommandHandler : IRequestHandler<SellAnimalCommand, AnimalResponse>
{
    private readonly IAnimalRepository _animalRepository;

    public SellAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<AnimalResponse> Handle(SellAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal == null)
            throw new KeyNotFoundException($"Animal with ID {request.AnimalId} not found.");

        animal.MarkAsSold(request.SalePrice, request.SaleDate, request.UserId);

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
            animal.ExternalFather,
            animal.Weight,
            animal.Height
        );
    }
}
