using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries;

public class GetBatchesByFarmQueryHandler : IRequestHandler<GetBatchesByFarmQuery, IEnumerable<BatchResponse>>
{
    private readonly IBatchRepository _batchRepository;

    public GetBatchesByFarmQueryHandler(IBatchRepository batchRepository)
    {
        _batchRepository = batchRepository;
    }

    public async Task<IEnumerable<BatchResponse>> Handle(GetBatchesByFarmQuery request, CancellationToken cancellationToken)
    {
        var batches = await _batchRepository.GetByFarmIdAsync(request.FarmId, false, cancellationToken);
        
        return batches.Select(batch => new BatchResponse(
            batch.Id,
            batch.Name,
            batch.FarmId
        ));
    }
}
