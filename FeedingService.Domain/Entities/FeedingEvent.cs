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
    public Money? CalculatedTotalCost { get; set; }
    public string? Observations { get; set; }
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

        if (UnitCostAtMoment.Amount < 0)
            throw new ArgumentException("UnitCostAtMoment cannot be negative");

        if (!BatchId.HasValue && !AnimalId.HasValue)
            throw new InvalidOperationException("Either BatchId or AnimalId must be provided");

        if (BatchId.HasValue && AnimalId.HasValue)
            throw new InvalidOperationException("Cannot specify both BatchId and AnimalId");
    }
}