using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Commands;

public class MoveAnimalToBatchCommandHandler : IRequestHandler<MoveAnimalToBatchCommand, AnimalResponse>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IBatchRepository _batchRepository;

    public MoveAnimalToBatchCommandHandler(IAnimalRepository animalRepository, IBatchRepository batchRepository)
    {
        _animalRepository = animalRepository;
        _batchRepository = batchRepository;
    }

    public async Task<AnimalResponse> Handle(MoveAnimalToBatchCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.AnimalId, cancellationToken);
        if (animal == null)
            throw new KeyNotFoundException($"Animal with ID {request.AnimalId} not found.");

        var batch = await _batchRepository.GetByIdAsync(request.BatchId, cancellationToken);
        if (batch == null)
            throw new KeyNotFoundException($"Batch with ID {request.BatchId} not found.");

        animal.MoveToBatch(request.BatchId, request.UserId);

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
