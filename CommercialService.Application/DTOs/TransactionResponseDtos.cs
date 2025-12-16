using System;
using CommercialService.Domain.Enums;

namespace CommercialService.Application.DTOs;

public class TransactionSummaryDto
{
    public long Id { get; set; }
    public int FarmId { get; set; }
    public long? ThirdPartyId { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? InvoiceNumber { get; set; }
    public decimal NetTotal { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}

public class TransactionFullDto : TransactionSummaryDto
{
    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Discounts { get; set; }
    public string? Observations { get; set; }
    public DateTime RegisteredAt { get; set; }
    public int? RegisteredBy { get; set; }
    // Could include lists of animals/products here if we want a single "Mega Query", 
    // but separate endpoints are often cleaner for large payloads.
}

public class TransactionAnimalDto
{
    public long Id { get; set; }
    public long AnimalId { get; set; }
    public decimal? PricePerKilo { get; set; }
    public decimal? WeightAtNegotiation { get; set; }
    public decimal BaseHeadPrice { get; set; }
    public decimal CommissionCost { get; set; }
    public decimal TransportCost { get; set; }
    public decimal? FinalLineValue { get; set; }
}

public class TransactionProductDto
{
    public long Id { get; set; }
    public int ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? LineSubtotal { get; set; }
}
