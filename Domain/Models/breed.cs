using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("name", Name = "breeds_name_key", IsUnique = true)]
public partial class breed
{
    [Key]
    public int id { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    [StringLength(200)]
    public string? description { get; set; }

    public bool? active { get; set; }

    [InverseProperty("breed")]
    public virtual ICollection<animal> animals { get; set; } = new List<animal>();
}
