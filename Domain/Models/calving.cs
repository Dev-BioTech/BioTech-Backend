using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", "calving_date", Name = "idx_calvings_date")]
public partial class calving
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long mother_id { get; set; }

    public DateOnly calving_date { get; set; }

    [StringLength(20)]
    public string? calving_type { get; set; }

    public int? number_of_calves { get; set; }

    [Precision(3, 1)]
    public decimal? body_condition { get; set; }

    public bool? placenta_retention { get; set; }

    public string? observations { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? registered_at { get; set; }

    [InverseProperty("calving")]
    public virtual ICollection<calving_calf> calving_calves { get; set; } = new List<calving_calf>();

    [ForeignKey("farm_id")]
    [InverseProperty("calvings")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("mother_id")]
    [InverseProperty("calvings")]
    public virtual animal mother { get; set; } = null!;
}
