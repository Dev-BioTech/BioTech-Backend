using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("v_low_stock_alerts")]
[Keyless]
public partial class VLowStockAlert
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("farm_id")]
    public int? FarmId { get; set; }

    [StringLength(100)]
    [Column("Farm")]
    public string? Farm { get; set; }

    [StringLength(100)]
    [Column("Product")]
    public string? Product { get; set; }

    [StringLength(50)]
    [Column("category")]
    public string? Category { get; set; }

    [Precision(12, 2)]
    [Column("current_quantity")]
    public decimal? CurrentQuantity { get; set; }

    [Precision(12, 2)]
    [Column("minimum_stock")]
    public decimal? MinimumStock { get; set; }

    [Column("deficit")]
    public decimal? Deficit { get; set; }

    [Column("alert_level")]
    public string? AlertLevel { get; set; }
}
