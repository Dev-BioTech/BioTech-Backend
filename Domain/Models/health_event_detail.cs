using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("health_event_details")]
public partial class HealthEventDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("health_event_id")]
    public long HealthEventId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Precision(8, 3)]
    [Column("dose_per_animal")]
    public decimal DosePerAnimal { get; set; }

    [Precision(10, 2)]
    [Column("total_quantity_deducted")]
    public decimal TotalQuantityDeducted { get; set; }

    [Precision(12, 2)]
    [Column("unit_cost_at_moment")]
    public decimal UnitCostAtMoment { get; set; }

    [Precision(12, 2)]
    [Column("calculated_total_cost")]
    public decimal? CalculatedTotalCost { get; set; }

    [StringLength(20)]
    [Column("administration_route")]
    public string? AdministrationRoute { get; set; }

    [ForeignKey("health_event_id")]
    [InverseProperty("health_event_details")]
    public virtual HealthEvent HealthEvent { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("health_event_details")]
    public virtual Product Product { get; set; } = null!;
}
