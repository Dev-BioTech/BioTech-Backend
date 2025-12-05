using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Keyless]
public partial class v_low_stock_alert
{
    public int? id { get; set; }

    public int? farm_id { get; set; }

    [StringLength(100)]
    public string? farm { get; set; }

    [StringLength(100)]
    public string? product { get; set; }

    [StringLength(50)]
    public string? category { get; set; }

    [Precision(12, 2)]
    public decimal? current_quantity { get; set; }

    [Precision(12, 2)]
    public decimal? minimum_stock { get; set; }

    public decimal? deficit { get; set; }

    public string? alert_level { get; set; }
}
