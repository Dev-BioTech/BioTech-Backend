using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.CreatePaddock;

public class CreatePaddockCommandHandler : IRequestHandler<CreatePaddockCommand, PaddockResponse>
{
    private readonly IPaddockRepository _repository;

    public CreatePaddockCommandHandler(IPaddockRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaddockResponse> Handle(CreatePaddockCommand request, CancellationToken cancellationToken)
    {
        var paddock = new Paddock
        {
            Name = request.Name,
            FarmId = request.FarmId
        };

        var createdPaddock = await _repository.AddAsync(paddock, cancellationToken);

        return new PaddockResponse(createdPaddock.Id, createdPaddock.Name, createdPaddock.FarmId);
    }
}
