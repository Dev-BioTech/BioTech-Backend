using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("name", Name = "diseases_name_key", IsUnique = true)]
public partial class disease
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(50)]
    public string? type { get; set; }

    [StringLength(200)]
    public string? description { get; set; }

    [InverseProperty("disease_diagnosis")]
    public virtual ICollection<health_event> health_events { get; set; } = new List<health_event>();
}
