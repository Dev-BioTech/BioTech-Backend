using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryService.Domain.Enums;

namespace InventoryService.Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int FarmId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ProductCategory? Category { get; set; }

    [Required]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = string.Empty;

    [Column(TypeName = "numeric(12,2)")]
    public decimal CurrentQuantity { get; set; } = 0;

    [Column(TypeName = "numeric(12,2)")]
    public decimal AverageCost { get; set; } = 0;

    [Column(TypeName = "numeric(12,2)")]
    public decimal MinimumStock { get; set; } = 0;

    public bool Active { get; set; } = true;
}
