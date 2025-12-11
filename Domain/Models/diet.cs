using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("diets")]
public partial class Diet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    [StringLength(100)]
    [Column("objective")]
    public string? Objective { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Diet")]
    public virtual ICollection<DietDetail> DietDetails { get; set; } = new List<DietDetail>();

    [ForeignKey("farm_id")]
    [InverseProperty("diets")]
    public virtual Farm Farm { get; set; } = null!;

    [InverseProperty("Diet")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();
}
