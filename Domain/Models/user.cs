using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("email", Name = "users_email_key", IsUnique = true)]
public partial class user
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    public string email { get; set; } = null!;

    [StringLength(255)]
    public string password_hash { get; set; } = null!;

    [StringLength(150)]
    public string full_name { get; set; } = null!;

    public bool? active { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? created_at { get; set; }

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<animal_movement> animal_movements { get; set; } = new List<animal_movement>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<commercial_transaction> commercial_transactions { get; set; } = new List<commercial_transaction>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<inventory_movement> inventory_movements { get; set; } = new List<inventory_movement>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<reproduction_event> reproduction_events { get; set; } = new List<reproduction_event>();

    [InverseProperty("user")]
    public virtual ICollection<user_farm_role> user_farm_roles { get; set; } = new List<user_farm_role>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<weighing> weighings { get; set; } = new List<weighing>();
}
