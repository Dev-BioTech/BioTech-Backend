using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Domain.Enums;

namespace HerdService.Application.Commands.CreateAnimal;

public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, AnimalResponse>
{
    private readonly IAnimalRepository _repository;

    public CreateAnimalCommandHandler(IAnimalRepository repository)
    {
        _repository = repository;
    }

    public async Task<AnimalResponse> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = new Animal
        {
            FarmId = request.FarmId,
            Identifier = request.Identifier,
            BirthDate = request.BirthDate,
            BreedId = request.BreedId,
            CategoryId = request.CategoryId,
            BatchId = request.BatchId,
            PaddockId = request.PaddockId,
            Gender = (Gender)request.Gender,
            Status = (AnimalStatus)request.Status,
            CreatedBy = request.RegisteredBy
        };

        animal.Validate();

        var createdAnimal = await _repository.AddAsync(animal, cancellationToken);

        return new AnimalResponse(
            createdAnimal.Id,
            createdAnimal.FarmId,
            createdAnimal.Identifier,
            createdAnimal.BirthDate,
            createdAnimal.BreedId,
            createdAnimal.CategoryId,
            createdAnimal.BatchId,
            createdAnimal.PaddockId,
            (int)createdAnimal.Gender,
            (int)createdAnimal.Status,
            createdAnimal.CreatedAt,
            createdAnimal.CreatedBy
        );
    }
}
