using AuthService.Application.DTOs;
using MediatR;

namespace AuthService.Application.Commands.CreateFarm;

public record CreateFarmCommand(
    string Name,
    string? Owner,
    string? Address,
    string? GeographicLocation,
    int? UserId
) : IRequest<FarmResponse>;
