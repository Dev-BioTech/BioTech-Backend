using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Index("batch_id", Name = "idx_animals_batch")]
[Index("farm_id", "current_status", Name = "idx_animals_farm_status")]
[Index("paddock_id", Name = "idx_animals_paddock")]
[Index("farm_id", "visual_code", Name = "uk_animal_code_farm", IsUnique = true)]
public partial class Animal
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(20)]
    [Column("visual_code")]
    public string VisualCode { get; set; } = null!;

    [StringLength(50)]
    [Column("electronic_code")]
    public string? ElectronicCode { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string? Name { get; set; }

    [Column("birth_date")]
    public DateOnly BirthDate { get; set; }

    [MaxLength(1)]
    [Column("sex")]
    public char Sex { get; set; }

    [Column("breed_id")]
    public int? BreedId { get; set; }

    [StringLength(30)]
    [Column("color")]
    public string? Color { get; set; }

    [Column("mother_id")]
    public long? MotherId { get; set; }

    [Column("father_id")]
    public long? FatherId { get; set; }

    [StringLength(50)]
    [Column("external_mother")]
    public string? ExternalMother { get; set; }

    [StringLength(50)]
    [Column("external_father")]
    public string? ExternalFather { get; set; }

    [Column("batch_id")]
    public int? BatchId { get; set; }

    [Column("paddock_id")]
    public int? PaddockId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [StringLength(20)]
    [Column("current_status")]
    public string? CurrentStatus { get; set; }

    [StringLength(20)]
    [Column("purpose")]
    public string? Purpose { get; set; }

    [StringLength(20)]
    [Column("origin")]
    public string? Origin { get; set; }

    [Column("entry_date")]
    public DateOnly? EntryDate { get; set; }

    [Precision(12, 2)]
    [Column("initial_cost")]
    public decimal? InitialCost { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("father")]
    public virtual ICollection<Animal> Inversefather { get; set; } = new List<Animal>();

    [InverseProperty("mother")]
    public virtual ICollection<Animal> Inversemother { get; set; } = new List<Animal>();

    [InverseProperty("animal")]
    public virtual ICollection<AnimalMovement> AnimalMovements { get; set; } = new List<AnimalMovement>();

    [ForeignKey("batch_id")]
    [InverseProperty("animals")]
    public virtual Batch? Batch { get; set; }

    [ForeignKey("breed_id")]
    [InverseProperty("animals")]
    public virtual Breed? Breed { get; set; }

    [InverseProperty("calf")]
    public virtual ICollection<CalvingCalf> CalvingCalves { get; set; } = new List<CalvingCalf>();

    [InverseProperty("mother")]
    public virtual ICollection<Calving> Calvings { get; set; } = new List<Calving>();

    [ForeignKey("category_id")]
    [InverseProperty("animals")]
    public virtual AnimalCategory? Category { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("animals")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("father_id")]
    [InverseProperty("Inversefather")]
    public virtual Animal? Father { get; set; }

    [InverseProperty("animal")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();

    [InverseProperty("animal")]
    public virtual ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();

    [InverseProperty("animal")]
    public virtual ICollection<MilkProduction> MilkProductions { get; set; } = new List<MilkProduction>();

    [ForeignKey("mother_id")]
    [InverseProperty("Inversemother")]
    public virtual Animal? Mother { get; set; }

    [ForeignKey("paddock_id")]
    [InverseProperty("animals")]
    public virtual Paddock? Paddock { get; set; }

    [InverseProperty("animal")]
    public virtual ICollection<ReproductionEvent> ReproductionEventanimals { get; set; } = new List<ReproductionEvent>();

    [InverseProperty("reproducer")]
    public virtual ICollection<ReproductionEvent> ReproductionEventreproducers { get; set; } = new List<ReproductionEvent>();

    [InverseProperty("animal")]
    public virtual ICollection<TransactionAnimalDetail> TransactionAnimalDetails { get; set; } = new List<TransactionAnimalDetail>();

    [InverseProperty("animal")]
    public virtual ICollection<Weighing> Weighings { get; set; } = new List<Weighing>();

    [InverseProperty("animal")]
    public virtual ICollection<WithdrawalPeriod> WithdrawalPeriods { get; set; } = new List<WithdrawalPeriod>();
}
