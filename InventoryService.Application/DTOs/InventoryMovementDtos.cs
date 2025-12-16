using InventoryService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Application.DTOs;

public class InventoryMovementDto
{
    public long Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty; // ENTRY/EXIT
    public string Concept { get; set; } = string.Empty; // PURCHASE/SALE...
    public decimal Quantity { get; set; }
    public DateTime MovementDate { get; set; }

    public string? ReferenceDocument { get; set; }
    public string? ReferenceType { get; set; }
    public string? Observations { get; set; }

    public decimal TransactionUnitCost { get; set; }
    public decimal? TransactionTotalCost { get; set; }

    public decimal? SubsequentQuantityBalance { get; set; }
    public decimal? SubsequentAverageCost { get; set; }

    public int? RegisteredBy { get; set; }
}

public class RegisterMovementDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public MovementConcept Concept { get; set; }

    public MovementDirection? Direction { get; set; } // Optional if Concept implies it

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }

    public long? ThirdPartyId { get; set; }
    public string? ReferenceDocument { get; set; }
    public string? ReferenceType { get; set; } // Added back for backwards compatibility/internal use
    public string? Observations { get; set; }

    public decimal? UnitCost { get; set; }

    public int UserId { get; set; }
}
