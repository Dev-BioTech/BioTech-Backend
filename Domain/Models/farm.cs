using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("farms")]
public partial class Farm
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    [Column("owner")]
    public string? Owner { get; set; }

    [StringLength(200)]
    [Column("address")]
    public string? Address { get; set; }

    [StringLength(100)]
    [Column("geographic_location")]
    public string? GeographicLocation { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [InverseProperty("Farm")]
    public virtual ICollection<AnimalMovement> AnimalMovements { get; set; } = new List<AnimalMovement>();

    [InverseProperty("Farm")]
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    [InverseProperty("Farm")]
    public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();

    [InverseProperty("Farm")]
    public virtual ICollection<Calving> Calvings { get; set; } = new List<Calving>();

    [InverseProperty("Farm")]
    public virtual ICollection<CommercialTransaction> CommercialTransactions { get; set; } = new List<CommercialTransaction>();

    [InverseProperty("Farm")]
    public virtual ICollection<Diet> Diets { get; set; } = new List<Diet>();

    [InverseProperty("Farm")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();

    [InverseProperty("Farm")]
    public virtual ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();

    [InverseProperty("Farm")]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    [InverseProperty("Farm")]
    public virtual ICollection<MilkProduction> MilkProductions { get; set; } = new List<MilkProduction>();

    [InverseProperty("Farm")]
    public virtual ICollection<Paddock> Paddocks { get; set; } = new List<Paddock>();

    [InverseProperty("Farm")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Farm")]
    public virtual ICollection<ReproductionEvent> ReproductionEvents { get; set; } = new List<ReproductionEvent>();

    [InverseProperty("Farm")]
    public virtual ICollection<ThirdParty> ThirdParties { get; set; } = new List<ThirdParty>();

    [InverseProperty("Farm")]
    public virtual ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();

    [InverseProperty("Farm")]
    public virtual ICollection<Weighing> Weighings { get; set; } = new List<Weighing>();

    [InverseProperty("Farm")]
    public virtual ICollection<WithdrawalPeriod> WithdrawalPeriods { get; set; } = new List<WithdrawalPeriod>();
}
