using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("health_events")]
[Index("farm_id", "event_date", Name = "idx_health_date")]
public partial class HealthEvent
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("event_date")]
    public DateOnly EventDate { get; set; }

    [StringLength(30)]
    [Column("event_type")]
    public string EventType { get; set; } = null!;

    [Column("batch_id")]
    public int? BatchId { get; set; }

    [Column("animal_id")]
    public long? AnimalId { get; set; }

    [Column("disease_diagnosis_id")]
    public int? DiseaseDiagnosisId { get; set; }

    [Column("professional_id")]
    public long? ProfessionalId { get; set; }

    [Precision(10, 2)]
    [Column("service_cost")]
    public decimal? ServiceCost { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("health_events")]
    public virtual Animal? Animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("health_events")]
    public virtual Batch? Batch { get; set; }

    [ForeignKey("disease_diagnosis_id")]
    [InverseProperty("health_events")]
    public virtual Disease? DiseaseDiagnosis { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("health_events")]
    public virtual Farm Farm { get; set; } = null!;

    [InverseProperty("HealthEvent")]
    public virtual ICollection<HealthEventDetail> HealthEventDetails { get; set; } = new List<HealthEventDetail>();

    [ForeignKey("professional_id")]
    [InverseProperty("health_events")]
    public virtual ThirdParty? Professional { get; set; }
}
