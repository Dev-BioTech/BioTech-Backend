using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("id", "current_quantity", "minimum_stock", Name = "idx_products_minimum_stock")]
[Index("farm_id", "name", Name = "uk_product_name_farm", IsUnique = true)]
public partial class product
{
    [Key]
    public int id { get; set; }

    public int farm_id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(50)]
    public string? category { get; set; }

    [StringLength(20)]
    public string unit_of_measure { get; set; } = null!;

    [Precision(12, 2)]
    public decimal? current_quantity { get; set; }

    [Precision(12, 2)]
    public decimal? average_cost { get; set; }

    [Precision(12, 2)]
    public decimal? minimum_stock { get; set; }

    public bool? active { get; set; }

    [InverseProperty("product")]
    public virtual ICollection<diet_detail> diet_details { get; set; } = new List<diet_detail>();

    [ForeignKey("farm_id")]
    [InverseProperty("products")]
    public virtual farm farm { get; set; } = null!;

    [InverseProperty("product")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();

    [InverseProperty("product")]
    public virtual ICollection<health_event_detail> health_event_details { get; set; } = new List<health_event_detail>();

    [InverseProperty("product")]
    public virtual ICollection<inventory_movement> inventory_movements { get; set; } = new List<inventory_movement>();

    [InverseProperty("product")]
    public virtual ICollection<transaction_product_detail> transaction_product_details { get; set; } = new List<transaction_product_detail>();
}
