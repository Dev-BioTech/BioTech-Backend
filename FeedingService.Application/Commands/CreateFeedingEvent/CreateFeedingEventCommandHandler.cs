using FeedingService.Application.DTOs;
using FeedingService.Application.Interfaces;
using FeedingService.Domain.Entities;
using FeedingService.Domain.ValueObjects;
using MediatR;

namespace FeedingService.Application.Commands.CreateFeedingEvent;

public class CreateFeedingEventCommandHandler 
    : IRequestHandler<CreateFeedingEventCommand, FeedingEventResponse>
{
    private readonly IFeedingEventRepository _repository;

    public CreateFeedingEventCommandHandler(IFeedingEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<FeedingEventResponse> Handle(
        CreateFeedingEventCommand request, 
        CancellationToken ct)
    {
        var feedingEvent = new FeedingEvent
        {
            FarmId = request.FarmId,
            SupplyDate = request.SupplyDate.Date,
            DietId = request.DietId,
            BatchId = request.BatchId,
            AnimalId = request.AnimalId,
            ProductId = request.ProductId,
            TotalQuantity = request.TotalQuantity,
            AnimalsFedCount = request.AnimalsFedCount,
            UnitCostAtMoment = Money.FromDecimal(request.UnitCostAtMoment),
            Observations = request.Observations,
            RegisteredBy = request.RegisteredBy,
            CreatedBy = request.UserId
        };

        feedingEvent.Validate();
        feedingEvent.CalculateCost();

        var created = await _repository.AddAsync(feedingEvent, ct);

        return MapToResponse(created);
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