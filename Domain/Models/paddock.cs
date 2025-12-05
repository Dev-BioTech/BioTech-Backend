using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", Name = "idx_paddocks_farm")]
[Index("farm_id", "code", Name = "uk_paddock_code_farm", IsUnique = true)]
public partial class paddock
{
    [Key]
    public int id { get; set; }

    public int farm_id { get; set; }

    [StringLength(20)]
    public string code { get; set; } = null!;

    [StringLength(50)]
    public string name { get; set; } = null!;

    [Precision(10, 2)]
    public decimal area_hectares { get; set; }

    public int? gauged_capacity { get; set; }

    [StringLength(50)]
    public string? grass_type { get; set; }

    [StringLength(20)]
    public string? current_status { get; set; }

    [InverseProperty("new_paddock")]
    public virtual ICollection<animal_movement> animal_movementnew_paddocks { get; set; } = new List<animal_movement>();

    [InverseProperty("previous_paddock")]
    public virtual ICollection<animal_movement> animal_movementprevious_paddocks { get; set; } = new List<animal_movement>();

    [InverseProperty("paddock")]
    public virtual ICollection<animal> animals { get; set; } = new List<animal>();

    [ForeignKey("farm_id")]
    [InverseProperty("paddocks")]
    public virtual farm farm { get; set; } = null!;
}
