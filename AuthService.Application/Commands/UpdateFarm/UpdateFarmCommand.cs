using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Commands.UpdateFarm;

public record UpdateFarmCommand(
    int Id,
    string Name,
    string? Owner,
    string? Address,
    string? GeographicLocation,
    int? UserId
) : IRequest<FarmResponse?>;
