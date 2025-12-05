using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", "concept", Name = "idx_kardex_concept")]
[Index("product_id", "movement_date", Name = "idx_kardex_product_date", IsDescending = new[] { false, true })]
public partial class inventory_movement
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public int product_id { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? movement_date { get; set; }

    [StringLength(10)]
    public string movement_type { get; set; } = null!;

    [StringLength(30)]
    public string concept { get; set; } = null!;

    [Precision(12, 2)]
    public decimal quantity { get; set; }

    [Precision(12, 2)]
    public decimal transaction_unit_cost { get; set; }

    [Precision(12, 2)]
    public decimal? transaction_total_cost { get; set; }

    [Precision(12, 2)]
    public decimal? subsequent_quantity_balance { get; set; }

    [Precision(12, 2)]
    public decimal? subsequent_average_cost { get; set; }

    public long? third_party_id { get; set; }

    [StringLength(50)]
    public string? reference_document { get; set; }

    [StringLength(255)]
    public string? observations { get; set; }

    public int? registered_by { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("inventory_movements")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("inventory_movements")]
    public virtual product product { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("inventory_movements")]
    public virtual user? registered_byNavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("inventory_movements")]
    public virtual third_party? third_party { get; set; }

    [InverseProperty("inventory_movement")]
    public virtual ICollection<transaction_product_detail> transaction_product_details { get; set; } = new List<transaction_product_detail>();
}
