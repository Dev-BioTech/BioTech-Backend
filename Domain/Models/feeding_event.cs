using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("batch_id", Name = "idx_feeding_events_batch")]
[Index("farm_id", "supply_date", Name = "idx_feeding_events_farm_date")]
public partial class feeding_event
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public DateOnly supply_date { get; set; }

    public int? diet_id { get; set; }

    public int? batch_id { get; set; }

    public long? animal_id { get; set; }

    public int product_id { get; set; }

    [Precision(12, 2)]
    public decimal total_quantity { get; set; }

    public int animals_fed_count { get; set; }

    [Precision(12, 2)]
    public decimal unit_cost_at_moment { get; set; }

    [Precision(12, 2)]
    public decimal? calculated_total_cost { get; set; }

    public string? observations { get; set; }

    public int? registered_by { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("feeding_events")]
    public virtual animal? animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("feeding_events")]
    public virtual batch? batch { get; set; }

    [ForeignKey("diet_id")]
    [InverseProperty("feeding_events")]
    public virtual diet? diet { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("feeding_events")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("feeding_events")]
    public virtual product product { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("feeding_events")]
    public virtual user? registered_byNavigation { get; set; }
}
