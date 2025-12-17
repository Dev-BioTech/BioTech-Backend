using HerdService.Domain.Common;

namespace HerdService.Domain.Entities;

public class Animal
{
    // Identity
    public long Id { get; private set; }
    public string VisualCode { get; private set; } = string.Empty; // Mapped to visual_code
    public string? ElectronicCode { get; private set; } // Mapped to electronic_code
    public string? Name { get; private set; } // Mapped to name

    // Classification
    public int FarmId { get; private set; }
    public int? BreedId { get; private set; }
    public int? CategoryId { get; private set; }
    public int? BatchId { get; private set; }
    public int? PaddockId { get; private set; }

    // Biological Information
    public DateOnly BirthDate { get; private set; }
    public string Sex { get; private set; } = "M"; // 'M' or 'F'
    public string? Color { get; private set; }

    // Genealogy
    public long? MotherId { get; private set; }
    public long? FatherId { get; private set; }
    public string? ExternalMother { get; private set; }
    public string? ExternalFather { get; private set; }

    // Status & Purpose
    public string CurrentStatus { get; private set; } = "ACTIVE"; // ACTIVE, SOLD, DEAD, CONSUMED
    public string Purpose { get; private set; } = "MEAT"; // MEAT, MILK, DUAL_PURPOSE, BREEDING
    public string? Origin { get; private set; } // BIRTH, PURCHASE
    public DateOnly? EntryDate { get; private set; }
    public decimal InitialCost { get; private set; } = 0;

    // Audit
    public DateTime CreatedAt { get; private set; }
    // UpdatedAt, CreatedBy, LastModifiedBy are NOT in the schema provided.

    // Navigation properties
    public virtual Breed? Breed { get; private set; }
    public virtual AnimalCategory? Category { get; private set; }
    public virtual Batch? Batch { get; private set; }
    public virtual Paddock? Paddock { get; private set; }
    public virtual Animal? Mother { get; private set; }
    public virtual Animal? Father { get; private set; }
    public virtual ICollection<AnimalMovement>? Movements { get; private set; }

    private Animal() { }

    public static Animal Create(
        string visualCode,
        int farmId,
        string sex,
        DateOnly birthDate,
        int? categoryId = null,
        int? breedId = null,
        string? name = null,
        string? electronicCode = null,
        string? color = null,
        string? purpose = "MEAT",
        string? origin = "BIRTH",
        decimal initialCost = 0,
        long? motherId = null,
        long? fatherId = null,
        string? externalMother = null,
        string? externalFather = null)
    {
        if (string.IsNullOrWhiteSpace(visualCode))
            throw new ArgumentException("Visual Code is required");

        if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Birth date cannot be in the future");

        var validSexes = new[] { "M", "F" };
        if (!validSexes.Contains(sex))
            throw new ArgumentException("Sex must be 'M' or 'F'");

        return new Animal
        {
            VisualCode = visualCode,
            FarmId = farmId,
            Sex = sex,
            BirthDate = birthDate,
            CategoryId = categoryId,
            BreedId = breedId,
            Name = name,
            ElectronicCode = electronicCode,
            Color = color,
            Purpose = purpose ?? "MEAT",
            Origin = origin,
            EntryDate = DateOnly.FromDateTime(DateTime.UtcNow),
            InitialCost = initialCost,
            CurrentStatus = "ACTIVE",
            CreatedAt = DateTime.UtcNow,
            MotherId = motherId,
            FatherId = fatherId,
            ExternalMother = externalMother,
            ExternalFather = externalFather
        };
    }

    public void UpdateDetails(
        string visualCode,
        string? electronicCode,
        string? name,
        string? color,
        int? breedId,
        int? categoryId,
        string? purpose,
        string? sex = null,
        DateOnly? birthDate = null,
        string? origin = null,
        decimal? initialCost = null,
        DateOnly? entryDate = null,
        long? motherId = null,
        long? fatherId = null,
        string? externalMother = null,
        string? externalFather = null)
    {
        if (string.IsNullOrWhiteSpace(visualCode)) throw new ArgumentException("Visual Code is required");

        VisualCode = visualCode;
        ElectronicCode = electronicCode;
        Name = name;
        Color = color;
        if (breedId.HasValue) BreedId = breedId.Value;
        if (categoryId.HasValue) CategoryId = categoryId.Value;
        if (!string.IsNullOrEmpty(purpose)) Purpose = purpose;

        if (!string.IsNullOrEmpty(sex))
        {
            var validSexes = new[] { "M", "F" };
            if (!validSexes.Contains(sex)) throw new ArgumentException("Sex must be 'M' or 'F'");
            Sex = sex;
        }

        if (birthDate.HasValue)
        {
            if (birthDate.Value > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ArgumentException("Birth date cannot be in the future");
            BirthDate = birthDate.Value;
        }

        if (!string.IsNullOrEmpty(origin)) Origin = origin;
        if (initialCost.HasValue) InitialCost = initialCost.Value;
        if (entryDate.HasValue) EntryDate = entryDate.Value;

        // Update Genealogy if provided (conceptually, null might mean 'remove' or 'no change'. 
        // For simplicity, let's assume we update if the argument is provided. 
        // But nullable arguments make "remove" hard. 
        // Let's stick to: if value is provided (not null), update it. 
        // If user wants to clear it, we might need specific methods or logic. 
        // For now, I will treat null as "no change" for IDs, but for strings maybe "set to value".
        // A better pattern for full update is to take the full state. 
        // Given the command sends everything, I'll update all if in the command.

        if (motherId.HasValue) MotherId = motherId.Value > 0 ? motherId.Value : null;
        if (fatherId.HasValue) FatherId = fatherId.Value > 0 ? fatherId.Value : null;
        if (externalMother != null) ExternalMother = externalMother;
        if (externalFather != null) ExternalFather = externalFather;
    }

    public void MoveToBatch(int batchId, int? userId)
    {
        if (CurrentStatus != "ACTIVE")
            throw new InvalidOperationException("Cannot move an inactive animal");

        BatchId = batchId;
        // Audit ignored as per schema
    }

    public void MoveToPaddock(int paddockId, int? userId)
    {
        if (CurrentStatus != "ACTIVE")
            throw new InvalidOperationException("Cannot move an inactive animal");

        PaddockId = paddockId;
    }

    public int GetAgeInMonths()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var months = ((today.Year - BirthDate.Year) * 12) + today.Month - BirthDate.Month;
        return months < 0 ? 0 : months;
    }

    public void MarkAsSold(decimal salePrice, DateOnly saleDate, int? userId)
    {
        // SalePrice not stored in animals table per schema
        ChangeStatus("SOLD");
    }

    public void MarkAsDead(DateOnly deathDate, string? reason, int? userId)
    {
        ChangeStatus("DEAD");
        // Notes not in schema? Missing 'notes' column.
        // If schema has 'observations' in 'calvings' but not animals? 
        // User schema provided `animals` does NOT have notes/observations.
    }

    public void Transfer(DateOnly transferDate, int? userId)
    {
        // Status 'Transferred' not in check constraint?
        // Check constraint: 'ACTIVE', 'SOLD', 'DEAD', 'CONSUMED'
        // 'Transferred' is not valid.
        // Maybe 'SOLD'? or 'CONSUMED'? Or just invalid operation?
        throw new InvalidOperationException("Transfer status not supported by current schema constraints");
    }

    public void AddMovement(int movementTypeId, int? toPaddockId, DateOnly date, string? notes, int? userId)
    {
        if (Movements == null) Movements = new List<AnimalMovement>();

        var movement = new AnimalMovement(
            farmId: FarmId,
            animalId: Id,
            movementTypeId: movementTypeId,
            movementDate: date,
            previousPaddockId: PaddockId,
            newPaddockId: toPaddockId,
            observations: notes,
            registeredBy: userId
        );
        Movements.Add(movement);

        if (toPaddockId.HasValue)
            PaddockId = toPaddockId;
    }

    public void ChangeStatus(string newStatus)
    {
        var validStatuses = new[] { "ACTIVE", "SOLD", "DEAD", "CONSUMED" };
        if (!validStatuses.Contains(newStatus))
            throw new ArgumentException($"Invalid status: {newStatus}");

        CurrentStatus = newStatus;
    }
}
