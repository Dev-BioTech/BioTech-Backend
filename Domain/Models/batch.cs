using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("batches")]
[Index("farm_id", Name = "idx_batches_farm")]
[Index("farm_id", "name", Name = "uk_batch_name_farm", IsUnique = true)]
public partial class Batch
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [InverseProperty("new_batch")]
    public virtual ICollection<AnimalMovement> AnimalMovementnewBatches { get; set; } = new List<AnimalMovement>();

    [InverseProperty("previous_batch")]
    public virtual ICollection<AnimalMovement> AnimalMovementpreviousBatches { get; set; } = new List<AnimalMovement>();

    [InverseProperty("Batch")]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    [ForeignKey("farm_id")]
    [InverseProperty("batches")]
    public virtual Farm Farm { get; set; } = null!;

    [InverseProperty("Batch")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();

    [InverseProperty("Batch")]
    public virtual ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();

    [InverseProperty("Batch")]
    public virtual ICollection<MilkProduction> MilkProductions { get; set; } = new List<MilkProduction>();
}
