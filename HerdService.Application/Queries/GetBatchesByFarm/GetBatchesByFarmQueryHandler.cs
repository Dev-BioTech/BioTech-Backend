using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Queries.GetBatchesByFarm;

public class GetBatchesByFarmQueryHandler : IRequestHandler<GetBatchesByFarmQuery, IEnumerable<BatchResponse>>
{
    private readonly IBatchRepository _repository;

    public GetBatchesByFarmQueryHandler(IBatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BatchResponse>> Handle(GetBatchesByFarmQuery request, CancellationToken cancellationToken)
    {
        var batches = await _repository.GetByFarmIdAsync(request.FarmId, request.IncludeInactive, cancellationToken);
        return batches.Select(b => new BatchResponse(b.Id, b.Name, b.FarmId));
    }
}
