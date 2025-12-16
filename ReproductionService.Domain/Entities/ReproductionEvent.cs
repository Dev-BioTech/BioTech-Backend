using ReproductionService.Domain.Common;
using ReproductionService.Domain.Enums;
using ReproductionService.Domain.ValueObjects;

namespace ReproductionService.Domain.Entities;

public class ReproductionEvent : IAuditableEntity
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public DateTime EventDate { get; set; }
    public long AnimalId { get; set; }
    public ReproductionEventType EventType { get; set; }
    public string? Observations { get; set; }
    public Money? Cost { get; set; }
    public int? RegisteredBy { get; set; }

    // Specific fields for different event types could be added here or in subclasses/value objects
    // For simplicity, we'll keep it generic for now, maybe adding JSONB for specific data if needed later
    // But following FeedingService pattern, we keep it flat if possible.

    public int? SireId { get; set; } // For Insemination/Birth
    public bool? IsPregnant { get; set; } // For PregnancyCheck
    public int? OffspringCount { get; set; } // For Birth

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    public ReproductionEvent()
    {
        EventDate = DateTime.UtcNow.Date;
        CreatedAt = DateTime.UtcNow;
        Cost = Money.Zero();
    }

    public void Validate()
    {
        if (FarmId <= 0)
            throw new ArgumentException("FarmId must be greater than zero");

        if (AnimalId <= 0)
            throw new ArgumentException("AnimalId must be greater than zero");

        if (!Enum.IsDefined(typeof(ReproductionEventType), EventType))
            throw new ArgumentException("Invalid EventType");

        if (Cost != null && Cost.Amount < 0)
            throw new ArgumentException("Cost cannot be negative");
    }
}
