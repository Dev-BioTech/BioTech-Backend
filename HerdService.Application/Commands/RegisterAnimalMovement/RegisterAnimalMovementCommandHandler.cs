using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.RegisterAnimalMovement;

public class RegisterAnimalMovementCommandHandler : IRequestHandler<RegisterAnimalMovementCommand, AnimalMovementResponse>
{
    private readonly IAnimalMovementRepository _repository;
    private readonly IMovementTypeRepository _movementTypeRepository;

    public RegisterAnimalMovementCommandHandler(IAnimalMovementRepository repository, IMovementTypeRepository movementTypeRepository)
    {
        _repository = repository;
        _movementTypeRepository = movementTypeRepository;
    }

    public async Task<AnimalMovementResponse> Handle(RegisterAnimalMovementCommand request, CancellationToken cancellationToken)
    {
        var movement = new AnimalMovement
        {
            AnimalId = request.AnimalId,
            MovementTypeId = request.MovementTypeId,
            MovementDate = request.MovementDate,
            Observation = request.Observation
        };

        var createdMovement = await _repository.AddAsync(movement, cancellationToken);

        // Fetch movement type name for response
        var movementType = await _movementTypeRepository.GetByIdAsync(request.MovementTypeId, cancellationToken);
        string movementTypeName = movementType?.Name ?? "Unknown";

        return new AnimalMovementResponse(
            createdMovement.Id,
            createdMovement.AnimalId,
            createdMovement.MovementTypeId,
            movementTypeName,
            createdMovement.MovementDate,
            createdMovement.Observation
        );
    }
}
