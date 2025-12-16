using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.CreateMovementType;

public class CreateMovementTypeCommandHandler : IRequestHandler<CreateMovementTypeCommand, MovementTypeResponse>
{
    private readonly IMovementTypeRepository _repository;

    public CreateMovementTypeCommandHandler(IMovementTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<MovementTypeResponse> Handle(CreateMovementTypeCommand request, CancellationToken cancellationToken)
    {
        var movementType = new MovementType
        {
            Name = request.Name
        };

        var createdMovementType = await _repository.AddAsync(movementType, cancellationToken);

        return new MovementTypeResponse(createdMovementType.Id, createdMovementType.Name);
    }
}
