
namespace AuthService.Application.DTOs;

public record UpdateFarmRequest(
    string Name,
    string? Location,
    decimal? Size,
    string? SizeUnit,
    string? Description,
    bool IsActive
);
