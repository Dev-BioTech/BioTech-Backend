using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("calvings")]
[Index("farm_id", "calving_date", Name = "idx_calvings_date")]
public partial class Calving
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("mother_id")]
    public long MotherId { get; set; }

    [Column("calving_date")]
    public DateOnly CalvingDate { get; set; }

    [StringLength(20)]
    [Column("calving_type")]
    public string? CalvingType { get; set; }

    [Column("number_of_calves")]
    public int? NumberOfCalves { get; set; }

    [Precision(3, 1)]
    [Column("body_condition")]
    public decimal? BodyCondition { get; set; }

    [Column("placenta_retention")]
    public bool? PlacentaRetention { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? RegisteredAt { get; set; }

    [InverseProperty("Calving")]
    public virtual ICollection<CalvingCalf> CalvingCalves { get; set; } = new List<CalvingCalf>();

    [ForeignKey("farm_id")]
    [InverseProperty("calvings")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("mother_id")]
    [InverseProperty("calvings")]
    public virtual Animal Mother { get; set; } = null!;
}
