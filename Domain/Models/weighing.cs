using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("weighings")]
[Index("animal_id", "weighing_date", Name = "idx_weighings_animal", IsDescending = new[] { false, true })]
[Index("animal_id", "weighing_date", Name = "uk_weighing_day", IsUnique = true)]
public partial class Weighing
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("animal_id")]
    public long AnimalId { get; set; }

    [Column("weighing_date")]
    public DateOnly WeighingDate { get; set; }

    [Precision(6, 2)]
    [Column("weight_kg")]
    public decimal WeightKg { get; set; }

    [Column("previous_weighing_date")]
    public DateOnly? PreviousWeighingDate { get; set; }

    [Precision(6, 2)]
    [Column("previous_weight")]
    public decimal? PreviousWeight { get; set; }

    [Precision(5, 3)]
    [Column("daily_weight_gain")]
    public decimal? DailyWeightGain { get; set; }

    [StringLength(200)]
    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("weighings")]
    public virtual Animal Animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("weighings")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("weighings")]
    public virtual User? RegisteredBynavigation { get; set; }
}
