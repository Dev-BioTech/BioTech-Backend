using HealthService.Domain.Common;
using HealthService.Domain.ValueObjects;

namespace HealthService.Domain.Entities;

public class HealthEvent : IAuditableEntity
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty; // VACCINATION, DEWORMING, TREATMENT, LAB_TEST
    public int? BatchId { get; set; }
    public long? AnimalId { get; set; }
    public int? DiseaseDiagnosisId { get; set; }
    public long? ProfessionalId { get; set; }
    public Money ServiceCost { get; set; } = Money.Zero();
    public string? Observations { get; set; }

    // Navigation property
    public virtual ICollection<HealthEventDetail> Details { get; set; } = new List<HealthEventDetail>();

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    public HealthEvent()
    {
        EventDate = DateTime.UtcNow.Date;
        CreatedAt = DateTime.UtcNow;
    }

    public void Validate()
    {
        if (FarmId <= 0) throw new ArgumentException("FarmId must be greater than zero");
        if (string.IsNullOrWhiteSpace(EventType)) throw new ArgumentException("EventType is required");
        if (!BatchId.HasValue && !AnimalId.HasValue) throw new InvalidOperationException("Either BatchId or AnimalId must be provided");
    }
}
