using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("animal_id", "weighing_date", Name = "idx_weighings_animal", IsDescending = new[] { false, true })]
[Index("animal_id", "weighing_date", Name = "uk_weighing_day", IsUnique = true)]
public partial class weighing
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long animal_id { get; set; }

    public DateOnly weighing_date { get; set; }

    [Precision(6, 2)]
    public decimal weight_kg { get; set; }

    public DateOnly? previous_weighing_date { get; set; }

    [Precision(6, 2)]
    public decimal? previous_weight { get; set; }

    [Precision(5, 3)]
    public decimal? daily_weight_gain { get; set; }

    [StringLength(200)]
    public string? observations { get; set; }

    public int? registered_by { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("weighings")]
    public virtual animal animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("weighings")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("weighings")]
    public virtual user? registered_byNavigation { get; set; }
}
