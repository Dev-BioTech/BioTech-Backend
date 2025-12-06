using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("paddocks")]
[Index("farm_id", Name = "idx_paddocks_farm")]
[Index("farm_id", "code", Name = "uk_paddock_code_farm", IsUnique = true)]
public partial class Paddock
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(20)]
    [Column("code")]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Precision(10, 2)]
    [Column("area_hectares")]
    public decimal AreaHectares { get; set; }

    [Column("gauged_capacity")]
    public int? GaugedCapacity { get; set; }

    [StringLength(50)]
    [Column("grass_type")]
    public string? GrassType { get; set; }

    [StringLength(20)]
    [Column("current_status")]
    public string? CurrentStatus { get; set; }

    [InverseProperty("new_paddock")]
    public virtual ICollection<AnimalMovement> AnimalMovementnewPaddocks { get; set; } = new List<AnimalMovement>();

    [InverseProperty("previous_paddock")]
    public virtual ICollection<AnimalMovement> AnimalMovementpreviousPaddocks { get; set; } = new List<AnimalMovement>();

    [InverseProperty("Paddock")]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    [ForeignKey("farm_id")]
    [InverseProperty("paddocks")]
    public virtual Farm Farm { get; set; } = null!;
}
