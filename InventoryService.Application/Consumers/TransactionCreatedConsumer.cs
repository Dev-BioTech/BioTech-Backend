using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using InventoryService.Domain.Enums; // Restored
using InventoryService.Application.IntegrationEvents;
using MediatR;
using MassTransit;
using System.Threading.Tasks;
using System;

namespace InventoryService.Application.Consumers;

public class TransactionCreatedConsumer : IConsumer<TransactionCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly IAnimalRepository _animalRepo; // Added

    public TransactionCreatedConsumer(IMediator mediator, IAnimalRepository animalRepo) // Modified constructor
    {
        _mediator = mediator;
        _animalRepo = animalRepo; // Added
    }

    public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
    {
        var message = context.Message;

        // Map Commercial Type to Inventory Concept/Direction
        MovementConcept concept;
        MovementDirection direction;

        // Commercial enum: Purchase=0, Sale=1.
        if (string.Equals(message.Type, "Purchase", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(message.Type, "PURCHASE", StringComparison.OrdinalIgnoreCase))
        {
            concept = MovementConcept.PURCHASE;
            direction = MovementDirection.ENTRY;
        }
        else if (string.Equals(message.Type, "Sale", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(message.Type, "SALE", StringComparison.OrdinalIgnoreCase))
        {
            concept = MovementConcept.SALE;
            direction = MovementDirection.EXIT;
        }
        else
        {
            return;
        }

        foreach (var item in message.Items)
        {
            if (item.ItemType == "Product")
            {
                var cmd = new RegisterMovementCommand(new RegisterMovementDto
                {
                    ProductId = (int)item.EntityId,
                    Concept = concept,
                    Direction = direction,
                    Quantity = item.Quantity,
                    ReferenceDocument = message.TransactionId.ToString(),
                    ReferenceType = "CommercialTransaction",
                    Observations = $"Auto-generated from Commercial Transaction #{message.TransactionId}",
                    UserId = 0
                });

                await _mediator.Send(cmd);
            }
            else if (item.ItemType == "Animal") // Added handling for Animal items
            {
                var animal = await _animalRepo.GetByIdAsync(item.EntityId, context.CancellationToken);
                if (animal != null)
                {
                    if (concept == MovementConcept.PURCHASE)
                    {
                        // Update Cost and Status
                        animal.InitialCost = item.UnitPrice;
                        animal.CurrentStatus = "ACTIVE";
                    }
                    else if (concept == MovementConcept.SALE)
                    {
                        animal.CurrentStatus = "SOLD";
                    }

                    await _animalRepo.UpdateAsync(animal, context.CancellationToken);
                }
            }
        }
    }
}
