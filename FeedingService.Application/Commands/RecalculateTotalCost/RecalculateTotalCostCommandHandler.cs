using FeedingService.Application.DTOs;
using FeedingService.Application.Interfaces;
using FeedingService.Domain.Entities;
using MediatR;

namespace FeedingService.Application.Commands.RecalculateTotalCost;

public class RecalculateTotalCostCommandHandler : IRequestHandler<RecalculateTotalCostCommand, FeedingEventResponse>
{
    private readonly IFeedingEventRepository _repository;

    public RecalculateTotalCostCommandHandler(IFeedingEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<FeedingEventResponse> Handle(RecalculateTotalCostCommand request, CancellationToken ct)
    {
        var feedingEvent = await _repository.GetByIdAsync(request.Id, ct);

        if (feedingEvent == null)
            throw new KeyNotFoundException($"Feeding event with id {request.Id} not found");

        feedingEvent.CalculateCost();
        
        await _repository.UpdateAsync(feedingEvent, ct);

        return MapToResponse(feedingEvent);
    }

    private static FeedingEventResponse MapToResponse(FeedingEvent entity)
    {
        return new FeedingEventResponse(
            entity.Id,
            entity.FarmId,
            entity.SupplyDate,
            entity.DietId,
            entity.BatchId,
            entity.AnimalId,
            entity.ProductId,
            entity.TotalQuantity,
            entity.AnimalsFedCount,
            entity.UnitCostAtMoment.Amount,
            entity.CalculatedTotalCost?.Amount,
            entity.Observations,
            entity.RegisteredBy,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }
}
