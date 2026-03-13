
namespace AuthService.Application.DTOs;

public record UpdateFarmRequest(
    string Name,
    string? Owner,
    string? Address,
    string? GeographicLocation
);

