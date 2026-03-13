using AuthService.Application.Interfaces;
using MediatR;

namespace AuthService.Application.Commands.DeleteFarm;

public class DeleteFarmCommandHandler : IRequestHandler<DeleteFarmCommand, bool>
{
    private readonly IFarmRepository _repository;

    public DeleteFarmCommandHandler(IFarmRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFarmCommand request, CancellationToken ct)
    {
        var farm = await _repository.GetByIdAsync(request.Id, ct);
        if (farm == null) return false;

        // Note: Real implementaiton might check if UserId matches farm owner/tenant
        
        await _repository.DeleteAsync(request.Id, ct);
        return true;
    }
}
