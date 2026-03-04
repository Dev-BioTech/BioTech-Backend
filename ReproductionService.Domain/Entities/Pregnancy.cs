namespace ReproductionService.Domain.Entities;

public class Pregnancy
{
    public long Id { get; set; }
    public long AnimalId { get; set; }
    public int FarmId { get; set; }
    public DateOnly ExpectedBirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public Pregnancy()
    {
    }

    public Pregnancy(long animalId, int farmId, DateOnly expectedBirthDate)
    {
        if (animalId <= 0) throw new ArgumentException("AnimalId must be greater than zero");
        if (farmId <= 0) throw new ArgumentException("FarmId must be greater than zero");
        
        AnimalId = animalId;
        FarmId = farmId;
        ExpectedBirthDate = expectedBirthDate;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
