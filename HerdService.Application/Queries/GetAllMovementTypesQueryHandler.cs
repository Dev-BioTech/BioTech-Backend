using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries;

public class GetAllMovementTypesQueryHandler : IRequestHandler<GetAllMovementTypesQuery, IEnumerable<MovementTypeResponse>>
{
    private readonly IMovementTypeRepository _movementTypeRepository;

    public GetAllMovementTypesQueryHandler(IMovementTypeRepository movementTypeRepository)
    {
        _movementTypeRepository = movementTypeRepository;
    }

    public async Task<IEnumerable<MovementTypeResponse>> Handle(GetAllMovementTypesQuery request, CancellationToken cancellationToken)
    {
        var movementTypes = await _movementTypeRepository.GetAllAsync(cancellationToken);
        
        return movementTypes.Select(movementType => new MovementTypeResponse(
            movementType.Id,
            movementType.Name
        ));
    }
}
