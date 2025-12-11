namespace FeedingService.Application.DTOs;

public record CreateFeedingEventRequest(
    int FarmId,
    DateTime SupplyDate,
    int? DietId,
    int? BatchId,
    long? AnimalId,
    int ProductId,
    decimal TotalQuantity,
    int AnimalsFedCount,
    decimal UnitCostAtMoment,
    string? Observations,
    int? RegisteredBy
);