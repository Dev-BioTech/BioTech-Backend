namespace HerdService.Application.DTOs;

public record AnimalResponse(
    long Id,
    string VisualCode,
    string? ElectronicCode,
    string? Name,
    string? Color,
    int FarmId,
    int? BreedId,
    string? BreedName,
    int? CategoryId, // Nullable now in entity? Yes
    string? CategoryName,
    int? BatchId,
    string? BatchName,
    int? PaddockId,
    string? PaddockName,
    DateOnly BirthDate,
    int AgeInMonths,
    string Sex,
    string CurrentStatus,
    string Purpose,
    string? Origin,
    DateOnly? EntryDate,
    decimal InitialCost,
    bool IsActive,
    long? MotherId,
    long? FatherId,
    string? ExternalMother,
    string? ExternalFather,
    decimal? Weight,
    decimal? Height
);
