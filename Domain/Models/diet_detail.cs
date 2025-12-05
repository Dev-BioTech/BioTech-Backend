using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("diet_id", "product_id", Name = "uk_diet_product", IsUnique = true)]
public partial class diet_detail
{
    [Key]
    public int id { get; set; }

    public int diet_id { get; set; }

    public int product_id { get; set; }

    [Precision(10, 3)]
    public decimal quantity_per_animal { get; set; }

    [StringLength(20)]
    public string? frequency { get; set; }

    [ForeignKey("diet_id")]
    [InverseProperty("diet_details")]
    public virtual diet diet { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("diet_details")]
    public virtual product product { get; set; } = null!;
}
