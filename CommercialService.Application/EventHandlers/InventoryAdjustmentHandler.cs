using MediatR;
using CommercialService.Domain.Events;
using CommercialService.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CommercialService.Application.EventHandlers;

public class InventoryAdjustmentHandler : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<InventoryAdjustmentHandler> _logger;
    // Inject InventoryService or HttpClient here in the future

    public InventoryAdjustmentHandler(ILogger<InventoryAdjustmentHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event Received: Transaction {Id} of type {Type}", notification.TransactionId, notification.Type);

        if (notification.Type == TransactionType.SALE)
        {
            foreach (var item in notification.Items)
            {
                if (item.ItemType == "Product")
                {
                    _logger.LogInformation("Reducing Stock for Product {ProductId}: {Quantity}", item.EntityId, item.Quantity);
                    // Call InventoryService.ReduceStock(item.EntityId, item.Quantity)
                }
                else if (item.ItemType == "Animal")
                {
                    _logger.LogInformation("Removing Animal {AnimalId} from Inventory", item.EntityId);
                    // Call HerdService.MarkAsSold(item.EntityId)
                }
            }
        }
        else if (notification.Type == TransactionType.PURCHASE)
        {
            foreach (var item in notification.Items)
            {
                if (item.ItemType == "Product")
                {
                    _logger.LogInformation("Increasing Stock for Product {ProductId}: {Quantity}", item.EntityId, item.Quantity);
                    // Call InventoryService.AddStock(item.EntityId, item.Quantity)
                }
                else if (item.ItemType == "Animal")
                {
                    _logger.LogInformation("Adding Animal {AnimalId} to Inventory", item.EntityId);
                    // Call HerdService.RegisterAnimal(item.EntityId)
                }
            }
        }

        return Task.CompletedTask;
    }
}
