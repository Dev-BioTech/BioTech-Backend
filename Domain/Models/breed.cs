using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("breeds")]
[Index("name", Name = "breeds_name_key", IsUnique = true)]
public partial class Breed
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [InverseProperty("Breed")]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
