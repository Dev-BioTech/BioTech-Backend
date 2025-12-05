using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("animal_id", "end_date", Name = "idx_withdrawals_date")]
public partial class withdrawal_period
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long animal_id { get; set; }

    public DateOnly start_date { get; set; }

    public int withdrawal_days { get; set; }

    public DateOnly? end_date { get; set; }

    [StringLength(20)]
    public string? product_type { get; set; }

    [StringLength(100)]
    public string? reason { get; set; }

    public bool? active { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("withdrawal_periods")]
    public virtual animal animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("withdrawal_periods")]
    public virtual farm farm { get; set; } = null!;
}
