namespace HealthService.Application.DTOs;

public record RegisterHealthEventRequest(
    int FarmId,
    long? AnimalId,
    int? BatchId,
    string EventType,
    DateOnly EventDate,
    string? Disease,
    string? Treatment,
    string? Medication,
    decimal? Dosage,
    string? DosageUnit,
    string? VeterinarianName,
    decimal? Cost,
    string? Notes,
    DateOnly? NextFollowUpDate,
    bool RequiresFollowUp,
    string? FollowUpNotes
);

public record HealthEventResponse(
    long Id,
    int FarmId,
    long? AnimalId,
    int? BatchId,
    string EventType,
    DateOnly EventDate,
    string? Disease,
    string? Treatment,
    string? Medication,
    decimal? Dosage,
    string? DosageUnit,
    string? VeterinarianName,
    decimal? Cost,
    string? Notes,
    DateOnly? NextFollowUpDate,
    bool RequiresFollowUp,
    string? FollowUpNotes,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record HealthEventListResponse(
    IEnumerable<HealthEventResponse> HealthEvents,
    int TotalCount
);
