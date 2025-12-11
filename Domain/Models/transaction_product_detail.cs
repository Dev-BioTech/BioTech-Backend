using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("transaction_product_details")]
public partial class TransactionProductDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("transaction_id")]
    public long TransactionId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Precision(12, 2)]
    [Column("quantity")]
    public decimal Quantity { get; set; }

    [Precision(12, 2)]
    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Precision(12, 2)]
    [Column("line_subtotal")]
    public decimal? LineSubtotal { get; set; }

    [Column("inventory_movement_id")]
    public long? InventoryMovementId { get; set; }

    [ForeignKey("inventory_movement_id")]
    [InverseProperty("transaction_product_details")]
    public virtual InventoryMovement? InventoryMovement { get; set; }

    [ForeignKey("product_id")]
    [InverseProperty("transaction_product_details")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("transaction_id")]
    [InverseProperty("transaction_product_details")]
    public virtual CommercialTransaction Transaction { get; set; } = null!;
}
