
namespace AuthService.Application.DTOs;

public record CreateFarmRequest(
    string Name,
    string? Owner,
    string? Address,
    string? GeographicLocation
);
