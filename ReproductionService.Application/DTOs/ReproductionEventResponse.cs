using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.DTOs;

public record ReproductionEventResponse(
    long Id,
    int FarmId,
    DateTime EventDate,
    long AnimalId,
    ReproductionEventType EventType,
    string? Observations,
    int? MaleAnimalId,
    // int? SemenBatchId, // Removed
    bool? IsSuccessful, // Was PregnancyResult
    DateOnly? ExpectedBirthDate, // Added
    int? OffspringCount,
    bool IsCancelled,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    int? RegisteredBy
);
