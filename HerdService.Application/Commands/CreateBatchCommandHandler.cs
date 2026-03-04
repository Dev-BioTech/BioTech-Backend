using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, BatchResponse>
{
    private readonly IBatchRepository _batchRepository;

    public CreateBatchCommandHandler(IBatchRepository batchRepository)
    {
        _batchRepository = batchRepository;
    }

    public async Task<BatchResponse> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = new Batch(request.FarmId, request.Name, null);
        
        var createdBatch = await _batchRepository.AddAsync(batch, cancellationToken);
        
        return new BatchResponse(
            createdBatch.Id,
            createdBatch.Name,
            createdBatch.FarmId
        );
    }
}
