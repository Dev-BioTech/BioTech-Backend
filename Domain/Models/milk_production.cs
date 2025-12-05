using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("milk_production")]
[Index("animal_id", Name = "idx_milk_animal")]
[Index("farm_id", "milking_date", Name = "idx_milk_date")]
public partial class milk_production
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public DateOnly milking_date { get; set; }

    [StringLength(10)]
    public string? shift { get; set; }

    public long? animal_id { get; set; }

    public int? batch_id { get; set; }

    [Precision(6, 2)]
    public decimal liters_quantity { get; set; }

    [Precision(4, 2)]
    public decimal? fat_percentage { get; set; }

    [Precision(4, 2)]
    public decimal? protein_percentage { get; set; }

    public int? somatic_cells { get; set; }

    [StringLength(200)]
    public string? observations { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("milk_productions")]
    public virtual animal? animal { get; set; }

    [ForeignKey("batch_id")]
    [InverseProperty("milk_productions")]
    public virtual batch? batch { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("milk_productions")]
    public virtual farm farm { get; set; } = null!;
}
