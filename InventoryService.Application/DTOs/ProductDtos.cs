using InventoryService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public int FarmId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal CurrentQuantity { get; set; }
    public decimal AverageCost { get; set; }
    public decimal MinimumStock { get; set; }
    public bool Active { get; set; }
}

public class CreateProductDto
{
    [Required]
    public int FarmId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ProductCategory? Category { get; set; }

    [Required]
    [MaxLength(20)]
    public string UnitOfMeasure { get; set; } = string.Empty;

    public decimal MinimumStock { get; set; } = 0;

    // Optional initial values
    public decimal CurrentQuantity { get; set; } = 0;
    public decimal AverageCost { get; set; } = 0;
}

public class UpdateProductDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ProductCategory? Category { get; set; }

    [MaxLength(20)]
    public string? UnitOfMeasure { get; set; }

    public decimal? MinimumStock { get; set; }
}
