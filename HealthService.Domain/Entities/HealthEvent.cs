using HealthService.Domain.Common;

namespace HealthService.Domain.Entities;

public class Animal
{
    public long Id { get; set; }
}

public class Batch
{
    public int Id { get; set; }
}

public class HealthEvent : IAuditableEntity
{
    public long Id { get; set; }
    
    // References
    public int FarmId { get; set; }
    public long? AnimalId { get; set; }  // Can be null if batch-level event
    public int? BatchId { get; set; }     // Can be null if individual animal event
    
    // Event Information
    public string EventType { get; set; } = string.Empty;  // "Vaccination", "Treatment", "Diagnosis", etc.
    public DateOnly EventDate { get; set; }
    public string? Disease { get; set; }
    public string? Treatment { get; set; }
    public string? Medication { get; set; }
    public decimal? Dosage { get; set; }
    public string? DosageUnit { get; set; }  // "ml", "mg", "tablets", etc.
    public string? VeterinarianName { get; set; }
    public decimal? Cost { get; set; }
    public string? Notes { get; set; }
    
    // Follow-up
    public DateOnly? NextFollowUpDate { get; set; }
    public bool RequiresFollowUp { get; set; }
    public string? FollowUpNotes { get; set; }
    
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
    
    // Navigation properties (nullable, for EF Core)
    public virtual Animal? Animal { get; set; }
    public virtual Batch? Batch { get; set; }

    public void Validate()
    {
        if (FarmId <= 0)
            throw new ArgumentException("FarmId must be greater than zero");
            
        if (!AnimalId.HasValue && !BatchId.HasValue)
            throw new InvalidOperationException("Either AnimalId or BatchId must be provided");
            
        if (AnimalId.HasValue && BatchId.HasValue)
            throw new InvalidOperationException("Cannot specify both AnimalId and BatchId");
            
        if (string.IsNullOrWhiteSpace(EventType))
            throw new ArgumentException("EventType is required");
            
        if (EventDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("EventDate cannot be in the future");
            
        if (Cost.HasValue && Cost.Value < 0)
            throw new ArgumentException("Cost cannot be negative");
            
        var validTypes = new[] { "Vaccination", "Treatment", "Diagnosis", "Check-up", "Medication", "Surgery", "Injury", "Disease" };
        if (!validTypes.Contains(EventType))
            throw new ArgumentException($"Invalid EventType. Must be one of: {string.Join(", ", validTypes)}");
    }
}
