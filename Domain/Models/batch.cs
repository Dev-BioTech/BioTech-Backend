using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", Name = "idx_batches_farm")]
[Index("farm_id", "name", Name = "uk_batch_name_farm", IsUnique = true)]
public partial class batch
{
    [Key]
    public int id { get; set; }

    public int farm_id { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    [StringLength(200)]
    public string? description { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? created_at { get; set; }

    public bool? active { get; set; }

    [InverseProperty("new_batch")]
    public virtual ICollection<animal_movement> animal_movementnew_batches { get; set; } = new List<animal_movement>();

    [InverseProperty("previous_batch")]
    public virtual ICollection<animal_movement> animal_movementprevious_batches { get; set; } = new List<animal_movement>();

    [InverseProperty("batch")]
    public virtual ICollection<animal> animals { get; set; } = new List<animal>();

    [ForeignKey("farm_id")]
    [InverseProperty("batches")]
    public virtual farm farm { get; set; } = null!;

    [InverseProperty("batch")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();

    [InverseProperty("batch")]
    public virtual ICollection<health_event> health_events { get; set; } = new List<health_event>();

    [InverseProperty("batch")]
    public virtual ICollection<milk_production> milk_productions { get; set; } = new List<milk_production>();
}
