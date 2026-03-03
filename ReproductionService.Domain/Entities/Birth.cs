namespace ReproductionService.Domain.Entities;

public class Birth
{
    public long Id { get; set; }
    public long MotherAnimalId { get; set; }
    public int FarmId { get; set; }
    public string OffspringTag { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public decimal Weight { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Birth()
    {
    }

    public Birth(long motherAnimalId, int farmId, string offspringTag, DateTime birthDate, decimal weight, string gender)
    {
        if (motherAnimalId <= 0) throw new ArgumentException("MotherAnimalId must be greater than zero");
        if (farmId <= 0) throw new ArgumentException("FarmId must be greater than zero");
        if (string.IsNullOrWhiteSpace(offspringTag)) throw new ArgumentException("OffspringTag is required");
        if (weight <= 0) throw new ArgumentException("Weight must be greater than zero");
        if (string.IsNullOrWhiteSpace(gender) || (gender != "Male" && gender != "Female")) 
            throw new ArgumentException("Gender must be 'Male' or 'Female'");

        MotherAnimalId = motherAnimalId;
        FarmId = farmId;
        OffspringTag = offspringTag;
        BirthDate = birthDate;
        Weight = weight;
        Gender = gender;
        CreatedAt = DateTime.UtcNow;
    }
}
