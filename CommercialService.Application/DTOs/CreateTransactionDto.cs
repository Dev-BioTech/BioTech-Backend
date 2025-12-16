using System;
using System.Collections.Generic;
using CommercialService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommercialService.Application.DTOs;

public record CreateTransactionDto
{
    [Required]
    public int FarmId { get; set; }

    public long? ThirdPartyId { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string? InvoiceNumber { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Taxes { get; set; }
    public decimal Discounts { get; set; }

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.PENDING;
    public string? Observations { get; set; }

    public List<CreateAnimalDetailDto> AnimalDetails { get; set; } = new();
    public List<CreateProductDetailDto> ProductDetails { get; set; } = new();
}

public record CreateAnimalDetailDto
{
    [Required]
    public long AnimalId { get; set; }

    public decimal? PricePerKilo { get; set; }
    public decimal? WeightAtNegotiation { get; set; }

    [Required]
    public decimal BaseHeadPrice { get; set; }

    public decimal CommissionCost { get; set; }
    public decimal TransportCost { get; set; }
}

public record CreateProductDetailDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public decimal Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
}
