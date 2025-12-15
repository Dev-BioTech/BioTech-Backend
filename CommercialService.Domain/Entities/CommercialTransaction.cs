using System;
using System.Collections.Generic;
using CommercialService.Domain.Enums;

namespace CommercialService.Domain.Entities;

public class CommercialTransaction
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public long? ThirdPartyId { get; set; }

    public TransactionType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? InvoiceNumber { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Discounts { get; set; }
    public decimal? NetTotal { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;
    public string? Observations { get; set; }

    public int? RegisteredBy { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<TransactionAnimalDetail> AnimalDetails { get; set; } = new List<TransactionAnimalDetail>();
    public ICollection<TransactionProductDetail> ProductDetails { get; set; } = new List<TransactionProductDetail>();
}
