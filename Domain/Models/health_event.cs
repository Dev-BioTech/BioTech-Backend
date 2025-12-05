using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", "event_date", Name = "idx_health_date")]
public partial class health_event
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public DateOnly event_date { get; set; }

    [StringLength(30)]
    public string event_type { get; set; } = null!;

    public int? batch_id { get; set; }

    public long? animal_id { get; set; }

    public int? disease_diagnosis_id { get; set; }

    public long? professional_id { get; set; }

    [Precision(10, 2)]
    public decimal? service_cost { get; set; }

    public string? observations { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("health_events")]
    public virtual animal? animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("health_events")]
    public virtual batch? batch { get; set; }

    [ForeignKey("disease_diagnosis_id")]
    [InverseProperty("health_events")]
    public virtual disease? disease_diagnosis { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("health_events")]
    public virtual farm farm { get; set; } = null!;

    [InverseProperty("health_event")]
    public virtual ICollection<health_event_detail> health_event_details { get; set; } = new List<health_event_detail>();

    [ForeignKey("professional_id")]
    [InverseProperty("health_events")]
    public virtual third_party? professional { get; set; }
}
