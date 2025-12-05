using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class health_event_detail
{
    [Key]
    public long id { get; set; }

    public long health_event_id { get; set; }

    public int product_id { get; set; }

    [Precision(8, 3)]
    public decimal dose_per_animal { get; set; }

    [Precision(10, 2)]
    public decimal total_quantity_deducted { get; set; }

    [Precision(12, 2)]
    public decimal unit_cost_at_moment { get; set; }

    [Precision(12, 2)]
    public decimal? calculated_total_cost { get; set; }

    [StringLength(20)]
    public string? administration_route { get; set; }

    [ForeignKey("health_event_id")]
    [InverseProperty("health_event_details")]
    public virtual health_event health_event { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("health_event_details")]
    public virtual product product { get; set; } = null!;
}
