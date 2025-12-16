
namespace AuthService.Application.DTOs;

public record FarmResponse(
    int Id,
    string Name,
    string? Owner,
    string? Address,
    string? GeographicLocation,
    bool Active,
    DateTime CreatedAt
);
