using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("movement_types")]
[Index("name", Name = "movement_types_name_key", IsUnique = true)]
public partial class MovementType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("affects_inventory")]
    public bool? AffectsInventory { get; set; }

    [Column("inventory_sign")]
    public int? InventorySign { get; set; }

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("MovementType")]
    public virtual ICollection<AnimalMovement> AnimalMovements { get; set; } = new List<AnimalMovement>();
}
