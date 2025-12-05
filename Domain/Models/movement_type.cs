using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("name", Name = "movement_types_name_key", IsUnique = true)]
public partial class movement_type
{
    [Key]
    public int id { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    public bool? affects_inventory { get; set; }

    public int? inventory_sign { get; set; }

    [StringLength(200)]
    public string? description { get; set; }

    [InverseProperty("movement_type")]
    public virtual ICollection<animal_movement> animal_movements { get; set; } = new List<animal_movement>();
}
