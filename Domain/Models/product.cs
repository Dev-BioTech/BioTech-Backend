using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("products")]
[Index("id", "current_quantity", "minimum_stock", Name = "idx_products_minimum_stock")]
[Index("farm_id", "name", Name = "uk_product_name_farm", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Column("category")]
    public string? Category { get; set; }

    [StringLength(20)]
    [Column("unit_of_measure")]
    public string UnitOfMeasure { get; set; } = null!;

    [Precision(12, 2)]
    [Column("current_quantity")]
    public decimal? CurrentQuantity { get; set; }

    [Precision(12, 2)]
    [Column("average_cost")]
    public decimal? AverageCost { get; set; }

    [Precision(12, 2)]
    [Column("minimum_stock")]
    public decimal? MinimumStock { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<DietDetail> DietDetails { get; set; } = new List<DietDetail>();

    [ForeignKey("farm_id")]
    [InverseProperty("products")]
    public virtual Farm Farm { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<FeedingEvent> FeedingEvents { get; set; } = new List<FeedingEvent>();

    [InverseProperty("Product")]
    public virtual ICollection<HealthEventDetail> HealthEventDetails { get; set; } = new List<HealthEventDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    [InverseProperty("Product")]
    public virtual ICollection<TransactionProductDetail> TransactionProductDetails { get; set; } = new List<TransactionProductDetail>();
}
