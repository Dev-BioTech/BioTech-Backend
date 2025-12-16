using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryService.Application.DTOs;

public class AnimalDto
{
    public long Id { get; set; }
    public string VisualCode { get; set; } = string.Empty;
    public string? ElectronicCode { get; set; }
    public string? Name { get; set; }
    public int FarmId { get; set; }
    public int? CategoryId { get; set; }
    public string Sex { get; set; } = "M";
    public string CurrentStatus { get; set; } = "ACTIVE";
    public int? BreedId { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal InitialCost { get; set; }
}

public class CreateAnimalDto
{
    [Required]
    public int FarmId { get; set; }

    [Required]
    [MaxLength(20)]
    public string VisualCode { get; set; } = string.Empty;

    public string? ElectronicCode { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    [Required]
    [RegularExpression("^[MF]$", ErrorMessage = "Sex must be 'M' or 'F'")]
    public string Sex { get; set; } = "M";

    public int? BreedId { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    public decimal InitialCost { get; set; }

    // Optional parents
    public long? MotherId { get; set; }
    public long? FatherId { get; set; }
}
