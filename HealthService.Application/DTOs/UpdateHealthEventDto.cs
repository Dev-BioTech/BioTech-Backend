namespace HealthService.Application.DTOs;

public record UpdateHealthEventDto(
    string? Description,
    DateOnly? Date,
    string? Treatment,
    string? VeterinarianName
);
