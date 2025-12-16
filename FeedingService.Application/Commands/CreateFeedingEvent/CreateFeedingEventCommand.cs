using FeedingService.Application.DTOs;
using MediatR;

namespace FeedingService.Application.Commands.CreateFeedingEvent;

public record CreateFeedingEventCommand(
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
    int? RegisteredBy,
    int? UserId
) : IRequest<FeedingEventResponse>;
