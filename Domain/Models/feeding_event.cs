using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("feeding_events")]
[Index("batch_id", Name = "idx_feeding_events_batch")]
[Index("farm_id", "supply_date", Name = "idx_feeding_events_farm_date")]
public partial class FeedingEvent
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("supply_date")]
    public DateOnly SupplyDate { get; set; }

    [Column("diet_id")]
    public int? DietId { get; set; }

    [Column("batch_id")]
    public int? BatchId { get; set; }

    [Column("animal_id")]
    public long? AnimalId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Precision(12, 2)]
    [Column("total_quantity")]
    public decimal TotalQuantity { get; set; }

    [Column("animals_fed_count")]
    public int AnimalsFedCount { get; set; }

    [Precision(12, 2)]
    [Column("unit_cost_at_moment")]
    public decimal UnitCostAtMoment { get; set; }

    [Precision(12, 2)]
    [Column("calculated_total_cost")]
    public decimal? CalculatedTotalCost { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("feeding_events")]
    public virtual Animal? Animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("feeding_events")]
    public virtual Batch? Batch { get; set; }

    [ForeignKey("diet_id")]
    [InverseProperty("feeding_events")]
    public virtual Diet? Diet { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("feeding_events")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("feeding_events")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("feeding_events")]
    public virtual User? RegisteredBynavigation { get; set; }
}
