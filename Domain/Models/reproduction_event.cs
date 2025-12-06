using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("reproduction_events")]
[Index("animal_id", "event_date", Name = "idx_reproduction_animal_date")]
[Index("farm_id", "diagnosis_result", Name = "idx_reproduction_diagnosis")]
public partial class ReproductionEvent
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("animal_id")]
    public long AnimalId { get; set; }

    [Column("event_date")]
    public DateOnly EventDate { get; set; }

    [StringLength(30)]
    [Column("event_type")]
    public string EventType { get; set; } = null!;

    [StringLength(20)]
    [Column("service_type")]
    public string? ServiceType { get; set; }

    [Column("reproducer_id")]
    public long? ReproducerId { get; set; }

    [StringLength(50)]
    [Column("external_reproducer")]
    public string? ExternalReproducer { get; set; }

    [StringLength(20)]
    [Column("diagnosis_result")]
    public string? DiagnosisResult { get; set; }

    [Column("estimated_gestation_days")]
    public int? EstimatedGestationDays { get; set; }

    [Precision(10, 2)]
    [Column("event_cost")]
    public decimal? EventCost { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [Column("probable_calving_date")]
    public DateOnly? ProbableCalvingDate { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("reproduction_eventanimals")]
    public virtual Animal Animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("reproduction_events")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("reproduction_events")]
    public virtual User? RegisteredBynavigation { get; set; }

    [ForeignKey("reproducer_id")]
    [InverseProperty("reproduction_eventreproducers")]
    public virtual Animal? Reproducer { get; set; }
}
