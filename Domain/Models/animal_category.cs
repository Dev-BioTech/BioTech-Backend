using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("name", Name = "animal_categories_name_key", IsUnique = true)]
public partial class animal_category
{
    [Key]
    public int id { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    [MaxLength(1)]
    public char? sex { get; set; }

    public int? min_age_months { get; set; }

    public int? max_age_months { get; set; }

    [StringLength(200)]
    public string? description { get; set; }

    [InverseProperty("category")]
    public virtual ICollection<animal> animals { get; set; } = new List<animal>();
}
