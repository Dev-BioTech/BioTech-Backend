using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class RegisterAnimalCommandHandler : IRequestHandler<RegisterAnimalCommand, AnimalResponse>
{
    private readonly IAnimalRepository _animalRepository;

    public RegisterAnimalCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public async Task<AnimalResponse> Handle(RegisterAnimalCommand request, CancellationToken cancellationToken)
    {
        if (await _animalRepository.VisualCodeExistsAsync(request.VisualCode, request.FarmId, cancellationToken))
            throw new InvalidOperationException($"Animal with Visual Code {request.VisualCode} already exists in this farm.");

        var animal = Animal.Create(
            request.VisualCode,
            request.FarmId,
            request.Sex,
            request.BirthDate,
            request.CategoryId,
            request.BreedId,
            request.Name,
            request.ElectronicCode,
            request.Color,
            request.Purpose,
            request.Origin,
            request.InitialCost,
            request.MotherId,
            request.FatherId,
            request.ExternalMother,
            request.ExternalFather,
            request.Weight,
            request.Height
        );

        // Handle optional Batch/Paddock assignments
        if (request.BatchId.HasValue)
            animal.MoveToBatch(request.BatchId.Value, request.UserId);

        if (request.PaddockId.HasValue)
            animal.MoveToPaddock(request.PaddockId.Value, request.UserId);

        await _animalRepository.AddAsync(animal, cancellationToken);

        return new AnimalResponse(
            animal.Id,
            animal.VisualCode,
            animal.ElectronicCode,
            animal.Name,
            animal.Color,
            animal.FarmId,
            animal.BreedId,
            null,
            animal.CategoryId,
            null,
            animal.BatchId,
            null,
            animal.PaddockId,
            null,
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
