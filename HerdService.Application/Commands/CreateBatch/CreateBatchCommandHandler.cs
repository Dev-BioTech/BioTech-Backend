using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.CreateBatch;

public class CreateBatchCommandHandler : IRequestHandler<CreateBatchCommand, BatchResponse>
{
    private readonly IBatchRepository _repository;

    public CreateBatchCommandHandler(IBatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<BatchResponse> Handle(CreateBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = new Batch
        {
            Name = request.Name,
            FarmId = request.FarmId
        };

        var createdBatch = await _repository.AddAsync(batch, cancellationToken);

        return new BatchResponse(createdBatch.Id, createdBatch.Name, createdBatch.FarmId);
    }
}
