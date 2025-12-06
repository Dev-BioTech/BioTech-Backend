using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("users")]
[Index("email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(100)]
    [Column("email")]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [StringLength(150)]
    [Column("full_name")]
    public string FullName { get; set; } = null!;

    [Column("active")]
    public bool? Active { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<AnimalMovement> AnimalMovements { get; set; } = new List<AnimalMovement>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<CommercialTransaction> CommercialTransactions { get; set; } = new List<CommercialTransaction>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<ReproductionEvent> ReproductionEvents { get; set; } = new List<ReproductionEvent>();

    [InverseProperty("User")]
    public virtual ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();

    [InverseProperty("registered_byNavigation")]
    public virtual ICollection<Weighing> Weighings { get; set; } = new List<Weighing>();
}
