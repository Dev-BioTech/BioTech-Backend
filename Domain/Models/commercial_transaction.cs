using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("commercial_transactions")]
[Index("farm_id", "transaction_date", Name = "idx_transactions_date")]
[Index("third_party_id", Name = "idx_transactions_third_party")]
public partial class CommercialTransaction
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("third_party_id")]
    public long? ThirdPartyId { get; set; }

    [StringLength(10)]
    [Column("transaction_type")]
    public string TransactionType { get; set; } = null!;

    [Column("transaction_date")]
    public DateOnly TransactionDate { get; set; }

    [StringLength(50)]
    [Column("invoice_number")]
    public string? InvoiceNumber { get; set; }

    [Precision(12, 2)]
    [Column("subtotal")]
    public decimal Subtotal { get; set; }

    [Precision(12, 2)]
    [Column("taxes")]
    public decimal? Taxes { get; set; }

    [Precision(12, 2)]
    [Column("discounts")]
    public decimal? Discounts { get; set; }

    [Precision(12, 2)]
    [Column("net_total")]
    public decimal? NetTotal { get; set; }

    [StringLength(20)]
    [Column("payment_status")]
    public string? PaymentStatus { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? RegisteredAt { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("commercial_transactions")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("commercial_transactions")]
    public virtual User? RegisteredBynavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("commercial_transactions")]
    public virtual ThirdParty? ThirdParty { get; set; }

    [InverseProperty("transaction")]
    public virtual ICollection<TransactionAnimalDetail> TransactionAnimalDetails { get; set; } = new List<TransactionAnimalDetail>();

    [InverseProperty("transaction")]
    public virtual ICollection<TransactionProductDetail> TransactionProductDetails { get; set; } = new List<TransactionProductDetail>();
}
