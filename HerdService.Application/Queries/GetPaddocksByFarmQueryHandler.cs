using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries;

public class GetPaddocksByFarmQueryHandler : IRequestHandler<GetPaddocksByFarmQuery, IEnumerable<PaddockResponse>>
{
    private readonly IPaddockRepository _paddockRepository;

    public GetPaddocksByFarmQueryHandler(IPaddockRepository paddockRepository)
    {
        _paddockRepository = paddockRepository;
    }

    public async Task<IEnumerable<PaddockResponse>> Handle(GetPaddocksByFarmQuery request, CancellationToken cancellationToken)
    {
        var paddocks = await _paddockRepository.GetByFarmIdAsync(request.FarmId, cancellationToken);
        
        return paddocks.Select(paddock => new PaddockResponse(
            paddock.Id,
            paddock.Name,
            paddock.FarmId
        ));
    }
}
