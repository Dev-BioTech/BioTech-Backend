using AuthService.Application.Interfaces;
using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Commands.UpdateFarm;

public class UpdateFarmCommandHandler : IRequestHandler<UpdateFarmCommand, FarmResponse?>
{
    private readonly IFarmRepository _repository;

    public UpdateFarmCommandHandler(IFarmRepository repository)
    {
        _repository = repository;
    }

    public async Task<FarmResponse?> Handle(UpdateFarmCommand request, CancellationToken ct)
    {
        var farm = await _repository.GetByIdAsync(request.Id, ct);
        if (farm == null) return null;

        farm.Name = request.Name;
        farm.Owner = request.Owner;
        farm.Address = request.Address;
        farm.GeographicLocation = request.GeographicLocation;

        await _repository.UpdateAsync(farm, ct);

        return new FarmResponse(
            farm.Id,
            farm.Name,
            farm.Owner,
            farm.Address,
            farm.GeographicLocation,
            farm.Active,
            farm.CreatedAt
        );
    }
}

