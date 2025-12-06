using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("milk_production")]
[Index("animal_id", Name = "idx_milk_animal")]
[Index("farm_id", "milking_date", Name = "idx_milk_date")]
public partial class MilkProduction
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("milking_date")]
    public DateOnly MilkingDate { get; set; }

    [StringLength(10)]
    [Column("shift")]
    public string? Shift { get; set; }

    [Column("animal_id")]
    public long? AnimalId { get; set; }

    [Column("batch_id")]
    public int? BatchId { get; set; }

    [Precision(6, 2)]
    [Column("liters_quantity")]
    public decimal LitersQuantity { get; set; }

    [Precision(4, 2)]
    [Column("fat_percentage")]
    public decimal? FatPercentage { get; set; }

    [Precision(4, 2)]
    [Column("protein_percentage")]
    public decimal? ProteinPercentage { get; set; }

    [Column("somatic_cells")]
    public int? SomaticCells { get; set; }

    [StringLength(200)]
    [Column("observations")]
    public string? Observations { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("milk_productions")]
    public virtual Animal? Animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("milk_productions")]
    public virtual Batch? Batch { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("milk_productions")]
    public virtual Farm Farm { get; set; } = null!;
}
