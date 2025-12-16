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
    public int? RegisteredBy { get; set; }

    // Specific fields
    public int? MaleAnimalId { get; set; } // For NaturalMating
    public int? SemenBatchId { get; set; } // For Insemination
    public bool? PregnancyResult { get; set; } // For PregnancyCheck
    public int? OffspringCount { get; set; } // For Birth

    public bool IsCancelled { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    public ReproductionEvent()
    {
        // EF Core
    }

    public ReproductionEvent(
        int farmId,
        long animalId,
        ReproductionEventType eventType,
        DateTime eventDate,
        int? registeredBy,
        string? observations = null)
    {
        FarmId = new ExternalId(farmId);
        AnimalId = animalId; // Assuming AnimalId is long, ExternalId wrapper might need adjustment or just use validation here. 
                             // Requirements said "animal_id > 0".
        if (animalId <= 0) throw new ArgumentException("AnimalId must be greater than zero");

        EventType = eventType;
        EventDate = new EventDate(eventDate);
        RegisteredBy = registeredBy;
        Observations = new Observation(observations);
        CreatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (IsCancelled)
            throw new InvalidOperationException("Event is already cancelled");
        
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Validate()
    {
        if (FarmId <= 0) throw new ArgumentException("FarmId must be greater than zero");
        if (AnimalId <= 0) throw new ArgumentException("AnimalId must be greater than zero");
        if (EventDate > DateTime.UtcNow.Date) throw new ArgumentException("EventDate cannot be in the future");

        switch (EventType)
        {
            case ReproductionEventType.Insemination:
                if (!SemenBatchId.HasValue)
                    throw new InvalidOperationException("SemenBatchId is required for Insemination");
                break;

            case ReproductionEventType.NaturalMating:
                if (!MaleAnimalId.HasValue)
                    throw new InvalidOperationException("MaleAnimalId is required for NaturalMating");
                break;

            case ReproductionEventType.PregnancyCheck:
                if (!PregnancyResult.HasValue)
                    throw new InvalidOperationException("PregnancyResult is required for PregnancyCheck");
                break;

            case ReproductionEventType.Birth:
                if (!OffspringCount.HasValue || OffspringCount < 1)
                    throw new InvalidOperationException("OffspringCount must be >= 1 for Birth");
                break;
        }
    }
}
