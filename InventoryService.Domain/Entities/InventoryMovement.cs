using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryService.Domain.Enums;

namespace InventoryService.Domain.Entities;

public class InventoryMovement
{
    [Key]
    public long Id { get; set; }

    [Required]
    public int FarmId { get; set; }

    [Required]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    public DateTime MovementDate { get; set; } = DateTime.UtcNow;

    [Required]
    public MovementDirection MovementType { get; set; } // ENTRY / EXIT

    [Required]
    public MovementConcept Concept { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal Quantity { get; set; } // Absolute Value

    [Column(TypeName = "numeric(12,2)")]
    public decimal TransactionUnitCost { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal? TransactionTotalCost { get; set; }

    // Snapshot fields (Kardex)
    [Column(TypeName = "numeric(12,2)")]
    public decimal? SubsequentQuantityBalance { get; set; }

    [Column(TypeName = "numeric(12,2)")]
    public decimal? SubsequentAverageCost { get; set; }

    public long? ThirdPartyId { get; set; }

    [MaxLength(50)]
    public string? ReferenceDocument { get; set; }

    [MaxLength(255)]
    public string? Observations { get; set; }

    public int? RegisteredBy { get; set; }
}
