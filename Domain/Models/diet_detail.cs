using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("diet_details")]
[Index("diet_id", "product_id", Name = "uk_diet_product", IsUnique = true)]
public partial class DietDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("diet_id")]
    public int DietId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Precision(10, 3)]
    [Column("quantity_per_animal")]
    public decimal QuantityPerAnimal { get; set; }

    [StringLength(20)]
    [Column("frequency")]
    public string? Frequency { get; set; }

    [ForeignKey("diet_id")]
    [InverseProperty("diet_details")]
    public virtual Diet Diet { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("diet_details")]
    public virtual Product Product { get; set; } = null!;
}
