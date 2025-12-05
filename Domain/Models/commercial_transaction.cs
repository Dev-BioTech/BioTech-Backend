using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", "transaction_date", Name = "idx_transactions_date")]
[Index("third_party_id", Name = "idx_transactions_third_party")]
public partial class commercial_transaction
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long? third_party_id { get; set; }

    [StringLength(10)]
    public string transaction_type { get; set; } = null!;

    public DateOnly transaction_date { get; set; }

    [StringLength(50)]
    public string? invoice_number { get; set; }

    [Precision(12, 2)]
    public decimal subtotal { get; set; }

    [Precision(12, 2)]
    public decimal? taxes { get; set; }

    [Precision(12, 2)]
    public decimal? discounts { get; set; }

    [Precision(12, 2)]
    public decimal? net_total { get; set; }

    [StringLength(20)]
    public string? payment_status { get; set; }

    public string? observations { get; set; }

    public int? registered_by { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? registered_at { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("commercial_transactions")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("registered_by")]
    [InverseProperty("commercial_transactions")]
    public virtual user? registered_byNavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("commercial_transactions")]
    public virtual third_party? third_party { get; set; }

    [InverseProperty("transaction")]
    public virtual ICollection<transaction_animal_detail> transaction_animal_details { get; set; } = new List<transaction_animal_detail>();

    [InverseProperty("transaction")]
    public virtual ICollection<transaction_product_detail> transaction_product_details { get; set; } = new List<transaction_product_detail>();
}
