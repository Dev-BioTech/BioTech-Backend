using HerdService.Domain.Common;
using HerdService.Domain.Enums;

namespace HerdService.Domain.Entities;

public class Animal : IAuditableEntity
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    public int BreedId { get; set; }
    public int? CategoryId { get; set; }
    public int? BatchId { get; set; }
    public int? PaddockId { get; set; }

    public Gender Gender { get; set; }
    public AnimalStatus Status { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    // Navigation properties
    public Breed? BreedEntity { get; set; }
    public AnimalCategory? Category { get; set; }
    public Batch? Batch { get; set; }
    public Paddock? Paddock { get; set; }

    public Animal()
    {
        CreatedAt = DateTime.UtcNow;
        Status = AnimalStatus.Active;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Identifier))
            throw new ArgumentException("Identifier is required");

        if (FarmId <= 0)
            throw new ArgumentException("FarmId must be greater than zero");

        if (!Enum.IsDefined(typeof(Gender), Gender))
            throw new ArgumentException("Invalid Gender");

        if (!Enum.IsDefined(typeof(AnimalStatus), Status))
            throw new ArgumentException("Invalid Status");
    }
}
