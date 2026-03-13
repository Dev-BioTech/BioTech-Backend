using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class CreatePaddockCommandHandler : IRequestHandler<CreatePaddockCommand, PaddockResponse>
{
    private readonly IPaddockRepository _paddockRepository;

    public CreatePaddockCommandHandler(IPaddockRepository paddockRepository)
    {
        _paddockRepository = paddockRepository;
    }

    public async Task<PaddockResponse> Handle(CreatePaddockCommand request, CancellationToken cancellationToken)
    {
        // Generate a simple code from the name
        var code = request.Name.ToUpper().Replace(" ", "_").Substring(0, Math.Min(request.Name.Length, 10));
        
        var paddock = new Paddock(request.FarmId, request.Name, code, 0); // Area defaults to 0
        
        var createdPaddock = await _paddockRepository.AddAsync(paddock, cancellationToken);
        
        return new PaddockResponse(
            createdPaddock.Id,
            createdPaddock.Name,
            createdPaddock.FarmId
        );
    }
}
