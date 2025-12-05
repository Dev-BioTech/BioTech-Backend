using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class transaction_product_detail
{
    [Key]
    public long id { get; set; }

    public long transaction_id { get; set; }

    public int product_id { get; set; }

    [Precision(12, 2)]
    public decimal quantity { get; set; }

    [Precision(12, 2)]
    public decimal unit_price { get; set; }

    [Precision(12, 2)]
    public decimal? line_subtotal { get; set; }

    public long? inventory_movement_id { get; set; }

    [ForeignKey("inventory_movement_id")]
    [InverseProperty("transaction_product_details")]
    public virtual inventory_movement? inventory_movement { get; set; }

    [ForeignKey("product_id")]
    [InverseProperty("transaction_product_details")]
    public virtual product product { get; set; } = null!;

    [ForeignKey("transaction_id")]
    [InverseProperty("transaction_product_details")]
    public virtual commercial_transaction transaction { get; set; } = null!;
}
