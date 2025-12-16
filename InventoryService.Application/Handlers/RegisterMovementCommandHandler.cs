using InventoryService.Application.Commands;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Enums;
using InventoryService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Handlers;

public class RegisterMovementCommandHandler : IRequestHandler<RegisterMovementCommand, long>
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IProductRepository _productRepo;

    public RegisterMovementCommandHandler(IInventoryRepository inventoryRepo, IProductRepository productRepo)
    {
        _inventoryRepo = inventoryRepo;
        _productRepo = productRepo;
    }

    public async Task<long> Handle(RegisterMovementCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var product = await _productRepo.GetByIdAsync(dto.ProductId, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {dto.ProductId} not found.");
        }

        // Determine Direction based on Concept or Input
        MovementDirection direction;

        switch (dto.Concept)
        {
            case MovementConcept.PURCHASE:
            case MovementConcept.RETURN: // Assuming Customer Return
            case MovementConcept.INVENTORY_ADJUSTMENT when dto.Direction == MovementDirection.ENTRY:
                direction = MovementDirection.ENTRY;
                break;

            case MovementConcept.SALE:
            case MovementConcept.FEED_CONSUMPTION:
            case MovementConcept.HEALTH_CONSUMPTION:
            case MovementConcept.EXPIRATION:
            case MovementConcept.INVENTORY_ADJUSTMENT when dto.Direction == MovementDirection.EXIT:
                direction = MovementDirection.EXIT;
                break;

            case MovementConcept.INVENTORY_ADJUSTMENT:
                // If not specified, default to... or check signed quantity? 
                // Here assuming DTO quantity is positive. 
                // If direction is null, default to ENTRY if we can't guess, or throw.
                if (dto.Direction.HasValue) direction = dto.Direction.Value;
                else throw new ArgumentException("Direction must be specified for Inventory Adjustment");
                break;

            default:
                // Default based on provided Direction or throw
                if (dto.Direction.HasValue) direction = dto.Direction.Value;
                else throw new ArgumentException($"Cannot determine direction for concept {dto.Concept}");
                break;
        }

        decimal quantityChange = direction == MovementDirection.ENTRY ? dto.Quantity : -dto.Quantity;

        // Check stock availability for outgoing movements
        if (direction == MovementDirection.EXIT && (product.CurrentQuantity + quantityChange) < 0)
        {
            throw new InvalidOperationException($"Insufficient stock for Product {product.Name}. Current: {product.CurrentQuantity}, Requested: {dto.Quantity}");
        }

        // Update Product Stock
        product.CurrentQuantity += quantityChange;

        // Update Average Cost (Weighted Average) ONLY on PURCHASES (ENTRY) with Cost
        decimal transactionUnitCost = dto.UnitCost ?? product.AverageCost; // Default to current avg if not provided

        if (direction == MovementDirection.ENTRY && transactionUnitCost > 0)
        {
            // New Average = ((OldQty * OldAvg) + (InQty * InCost)) / NewQty
            // Note: OldQty here is before update? No, we just updated product.CurrentQuantity.
            // Formula: ((TotalValue + NewValue) / TotalQty)

            decimal oldTotalValue = (product.CurrentQuantity - quantityChange) * product.AverageCost;
            decimal newInValue = quantityChange * transactionUnitCost;

            if (product.CurrentQuantity > 0)
            {
                product.AverageCost = (oldTotalValue + newInValue) / product.CurrentQuantity;
            }
        }
        // For EXIT, Average Cost doesn't change, we just consume value.

        // Create Movement
        var movement = new InventoryMovement
        {
            FarmId = product.FarmId, // Inherit from Product
            ProductId = dto.ProductId,
            MovementType = direction,
            Concept = dto.Concept,
            Quantity = dto.Quantity,
            MovementDate = DateTime.UtcNow,
            ThirdPartyId = dto.ThirdPartyId,
            ReferenceDocument = dto.ReferenceDocument,
            Observations = dto.Observations,
            RegisteredBy = dto.UserId,

            TransactionUnitCost = transactionUnitCost,
            TransactionTotalCost = transactionUnitCost * dto.Quantity,

            SubsequentQuantityBalance = product.CurrentQuantity,
            SubsequentAverageCost = product.AverageCost
        };

        // Save
        await _inventoryRepo.AddMovementAsync(movement, cancellationToken);
        await _productRepo.UpdateAsync(product, cancellationToken);

        return movement.Id;
    }
}
