using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class farm
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(100)]
    public string? owner { get; set; }

    [StringLength(200)]
    public string? address { get; set; }

    [StringLength(100)]
    public string? geographic_location { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? created_at { get; set; }

    public bool? active { get; set; }

    [InverseProperty("farm")]
    public virtual ICollection<animal_movement> animal_movements { get; set; } = new List<animal_movement>();

    [InverseProperty("farm")]
    public virtual ICollection<animal> animals { get; set; } = new List<animal>();

    [InverseProperty("farm")]
    public virtual ICollection<batch> batches { get; set; } = new List<batch>();

    [InverseProperty("farm")]
    public virtual ICollection<calving> calvings { get; set; } = new List<calving>();

    [InverseProperty("farm")]
    public virtual ICollection<commercial_transaction> commercial_transactions { get; set; } = new List<commercial_transaction>();

    [InverseProperty("farm")]
    public virtual ICollection<diet> diets { get; set; } = new List<diet>();

    [InverseProperty("farm")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();

    [InverseProperty("farm")]
    public virtual ICollection<health_event> health_events { get; set; } = new List<health_event>();

    [InverseProperty("farm")]
    public virtual ICollection<inventory_movement> inventory_movements { get; set; } = new List<inventory_movement>();

    [InverseProperty("farm")]
    public virtual ICollection<milk_production> milk_productions { get; set; } = new List<milk_production>();

    [InverseProperty("farm")]
    public virtual ICollection<paddock> paddocks { get; set; } = new List<paddock>();

    [InverseProperty("farm")]
    public virtual ICollection<product> products { get; set; } = new List<product>();

    [InverseProperty("farm")]
    public virtual ICollection<reproduction_event> reproduction_events { get; set; } = new List<reproduction_event>();

    [InverseProperty("farm")]
    public virtual ICollection<third_party> third_parties { get; set; } = new List<third_party>();

    [InverseProperty("farm")]
    public virtual ICollection<user_farm_role> user_farm_roles { get; set; } = new List<user_farm_role>();

    [InverseProperty("farm")]
    public virtual ICollection<weighing> weighings { get; set; } = new List<weighing>();

    [InverseProperty("farm")]
    public virtual ICollection<withdrawal_period> withdrawal_periods { get; set; } = new List<withdrawal_period>();
}
