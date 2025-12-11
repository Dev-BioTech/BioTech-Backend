using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Index("name", Name = "animal_categories_name_key", IsUnique = true)]
public partial class AnimalCategory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [MaxLength(1)]
    [Column("sex")]
    public char? Sex { get; set; }

    [Column("min_age_months")]
    public int? MinAgeMonths { get; set; }

    [Column("max_age_months")]
    public int? MaxAgeMonths { get; set; }

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("category")]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
