using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("diseases")]
[Index("name", Name = "diseases_name_key", IsUnique = true)]
public partial class Disease
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Column("type")]
    public string? Type { get; set; }

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("disease_diagnosis")]
    public virtual ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();
}
