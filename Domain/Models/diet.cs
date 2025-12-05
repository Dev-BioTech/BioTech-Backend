using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class diet
{
    [Key]
    public int id { get; set; }

    public int farm_id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(255)]
    public string? description { get; set; }

    [StringLength(100)]
    public string? objective { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? created_at { get; set; }

    [InverseProperty("diet")]
    public virtual ICollection<diet_detail> diet_details { get; set; } = new List<diet_detail>();

    [ForeignKey("farm_id")]
    [InverseProperty("diets")]
    public virtual farm farm { get; set; } = null!;

    [InverseProperty("diet")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();
}
