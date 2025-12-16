using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Commands;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Handlers;

public class DeleteFarmHandler : IRequestHandler<DeleteFarmCommand, bool>
{
    private readonly IFarmRepository _farmRepository;

    public DeleteFarmHandler(IFarmRepository farmRepository)
    {
        _farmRepository = farmRepository;
    }

    public async Task<bool> Handle(DeleteFarmCommand request, CancellationToken cancellationToken)
    {
        var farm = await _farmRepository.GetByIdAsync(request.Id, cancellationToken);
        if (farm == null) return false;

        await _farmRepository.DeleteAsync(farm, cancellationToken);
        return true;
    }
}
