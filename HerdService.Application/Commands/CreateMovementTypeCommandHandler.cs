using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class CreateMovementTypeCommandHandler : IRequestHandler<CreateMovementTypeCommand, MovementTypeResponse>
{
    private readonly IMovementTypeRepository _repository;

    public CreateMovementTypeCommandHandler(IMovementTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<MovementTypeResponse> Handle(CreateMovementTypeCommand request, CancellationToken cancellationToken)
    {
        var movementType = new MovementType(
            request.Name,
            request.Description,
            request.AffectsInventory,
            request.InventorySign
        );

        var savedType = await _repository.AddAsync(movementType, cancellationToken);
        
        return new MovementTypeResponse(
            savedType.Id,
            savedType.Name
        );
    }
}
