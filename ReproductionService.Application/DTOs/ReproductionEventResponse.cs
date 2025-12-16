using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.DTOs;

public record ReproductionEventResponse(
    long Id,
    int FarmId,
    DateTime EventDate,
    long AnimalId,
    ReproductionEventType EventType,
    string? Observations,
    decimal? Cost,
    int? SireId,
    bool? IsPregnant,
    int? OffspringCount,
    DateTime CreatedAt,
    int? CreatedBy
);
