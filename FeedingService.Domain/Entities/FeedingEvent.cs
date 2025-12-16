using FeedingService.Domain.Common;
using FeedingService.Domain.ValueObjects;

namespace FeedingService.Domain.Entities;


public class FeedingEvent : IAuditableEntity
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public DateTime SupplyDate { get; set; }
    public int? DietId { get; set; }
    public int? BatchId { get; set; }
    public long? AnimalId { get; set; }
    public int ProductId { get; set; }
    public decimal TotalQuantity { get; set; }
    public int AnimalsFedCount { get; set; }
    public Money UnitCostAtMoment { get; set; }
    public Money? CalculatedTotalCost { get; private set; }
    public string? Observations { get; set; }
    public bool IsCancelled { get; private set; }
    public int? RegisteredBy { get; set; }
    
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    public FeedingEvent()
    {
        SupplyDate = DateTime.UtcNow.Date;
        CreatedAt = DateTime.UtcNow;
        UnitCostAtMoment = Money.Zero();
    }

    public void CalculateCost()
    {
        if (UnitCostAtMoment != null)
        {
            CalculatedTotalCost = UnitCostAtMoment * TotalQuantity;
        }
    }

    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Event is already cancelled");
            
        IsCancelled = true;
    }

    public void Validate()
    {
        if (FarmId <= 0)
            throw new ArgumentException("FarmId must be greater than zero");

        if (ProductId <= 0)
            throw new ArgumentException("ProductId must be greater than zero");

        if (TotalQuantity <= 0)
            throw new ArgumentException("TotalQuantity must be greater than zero");

        if (AnimalsFedCount <= 0)
            throw new ArgumentException("AnimalsFedCount must be greater than zero");

        // Money validation is handled by Value Object, but we check existence
        if (UnitCostAtMoment == null)
             throw new ArgumentNullException(nameof(UnitCostAtMoment));

        // XOR Validation: Exactly one must be true
        bool hasBatch = BatchId.HasValue;
        bool hasAnimal = AnimalId.HasValue;

        if (hasBatch == hasAnimal) // Both true or both false
            throw new InvalidOperationException("Exactly one of BatchId or AnimalId must be provided (XOR)");
    }
}