using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("animal_id", "event_date", Name = "idx_reproduction_animal_date")]
[Index("farm_id", "diagnosis_result", Name = "idx_reproduction_diagnosis")]
public partial class reproduction_event
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long animal_id { get; set; }

    public DateOnly event_date { get; set; }

    [StringLength(30)]
    public string event_type { get; set; } = null!;

    [StringLength(20)]
    public string? service_type { get; set; }

    public long? reproducer_id { get; set; }

    [StringLength(50)]
    public string? external_reproducer { get; set; }

    [StringLength(20)]
    public string? diagnosis_result { get; set; }

    public int? estimated_gestation_days { get; set; }

    [Precision(10, 2)]
    public decimal? event_cost { get; set; }

    public string? observations { get; set; }

    public int? registered_by { get; set; }

    public DateOnly? probable_calving_date { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("reproduction_eventanimals")]
    public virtual animal animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("reproduction_events")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("reproduction_events")]
    public virtual user? registered_byNavigation { get; set; }

    [ForeignKey("reproducer_id")]
    [InverseProperty("reproduction_eventreproducers")]
    public virtual animal? reproducer { get; set; }
}
