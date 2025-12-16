using FeedingService.Application.DTOs;
using FeedingService.Application.Interfaces;
using FeedingService.Domain.Entities;
using MediatR;

namespace FeedingService.Application.Queries.GetFeedingEventsByProduct;

public class GetFeedingEventsByProductQueryHandler : IRequestHandler<GetFeedingEventsByProductQuery, FeedingEventListResponse>
{
    private readonly IFeedingEventRepository _repository;

    public GetFeedingEventsByProductQueryHandler(IFeedingEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<FeedingEventListResponse> Handle(GetFeedingEventsByProductQuery request, CancellationToken ct)
    {
        var events = await _repository.GetByProductIdAsync(request.ProductId, ct);
        var responses = events.Select(MapToResponse).ToList();
        return new FeedingEventListResponse(responses, responses.Count);
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
