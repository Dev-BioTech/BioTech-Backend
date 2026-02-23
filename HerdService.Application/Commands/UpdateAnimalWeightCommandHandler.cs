using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Commands;

public class UpdateAnimalWeightCommandHandler : IRequestHandler<UpdateAnimalWeightCommand, AnimalResponse>
{
    private readonly IAnimalRepository _animalRepository;

    public UpdateAnimalWeightCommandHandler(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    public Task<AnimalResponse> Handle(UpdateAnimalWeightCommand request, CancellationToken cancellationToken)
    {
        // Schema mismatch: 'animals' table does not have weight columns.
        throw new NotImplementedException("Animal table does not support weight updates. Please use Calving/WeightService.");
    }
}
