namespace FeedingService.Application.DTOs;

public record FeedingEventResponse(
    long Id,
    int FarmId,
    DateTime SupplyDate,
    int? DietId,
    int? BatchId,
    long? AnimalId,
    int ProductId,
    decimal TotalQuantity,
    int AnimalsFedCount,
    decimal UnitCostAtMoment,
    decimal? CalculatedTotalCost,
    string? Observations,
    int? RegisteredBy,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
