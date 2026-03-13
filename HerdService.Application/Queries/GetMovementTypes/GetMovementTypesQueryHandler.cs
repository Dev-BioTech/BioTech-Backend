using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Queries.GetMovementTypes;

public class GetMovementTypesQueryHandler : IRequestHandler<GetMovementTypesQuery, IEnumerable<MovementTypeResponse>>
{
    private readonly IMovementTypeRepository _repository;

    public GetMovementTypesQueryHandler(IMovementTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MovementTypeResponse>> Handle(GetMovementTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _repository.GetAllAsync(cancellationToken);
        return types.Select(t => new MovementTypeResponse(t.Id, t.Name));
    }
}
