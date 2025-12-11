using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("inventory_movements")]
[Index("farm_id", "concept", Name = "idx_kardex_concept")]
[Index("product_id", "movement_date", Name = "idx_kardex_product_date", IsDescending = new[] { false, true })]
public partial class InventoryMovement
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? MovementDate { get; set; }

    [StringLength(10)]
    [Column("MovementType")]
    public string MovementType { get; set; } = null!;

    [StringLength(30)]
    [Column("concept")]
    public string Concept { get; set; } = null!;

    [Precision(12, 2)]
    [Column("quantity")]
    public decimal Quantity { get; set; }

    [Precision(12, 2)]
    [Column("transaction_unit_cost")]
    public decimal TransactionUnitCost { get; set; }

    [Precision(12, 2)]
    [Column("transaction_total_cost")]
    public decimal? TransactionTotalCost { get; set; }

    [Precision(12, 2)]
    [Column("subsequent_quantity_balance")]
    public decimal? SubsequentQuantityBalance { get; set; }

    [Precision(12, 2)]
    [Column("subsequent_average_cost")]
    public decimal? SubsequentAverageCost { get; set; }

    [Column("third_party_id")]
    public long? ThirdPartyId { get; set; }

    [StringLength(50)]
    [Column("reference_document")]
    public string? ReferenceDocument { get; set; }

    [StringLength(255)]
    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("inventory_movements")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("product_id")]
    [InverseProperty("inventory_movements")]
    public virtual Product Product { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("inventory_movements")]
    public virtual User? RegisteredBynavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("inventory_movements")]
    public virtual ThirdParty? ThirdParty { get; set; }

    [InverseProperty("InventoryMovement")]
    public virtual ICollection<TransactionProductDetail> TransactionProductDetails { get; set; } = new List<TransactionProductDetail>();
}
