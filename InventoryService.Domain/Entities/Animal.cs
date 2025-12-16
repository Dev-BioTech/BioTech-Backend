using InventoryService.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Domain.Entities;

public class Animal
{
    [Key]
    public long Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string VisualCode { get; set; } = string.Empty; // Mapped to visual_code

    [MaxLength(50)]
    public string? ElectronicCode { get; set; } // Mapped to electronic_code

    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    public int FarmId { get; set; }

    public int? CategoryId { get; set; } // Mapped to category_id

    [MaxLength(1)]
    public string Sex { get; set; } = "M"; // Mapped to sex (M/F)

    [MaxLength(20)]
    public string CurrentStatus { get; set; } = "ACTIVE"; // Mapped to current_status

    public int? BreedId { get; set; } // Mapped to breed_id

    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; } // Mapped to birth_date

    // Genealogy
    public long? MotherId { get; set; }
    public long? FatherId { get; set; }

    // Costing
    [Column(TypeName = "numeric(12,2)")]
    public decimal InitialCost { get; set; } // Mapped to initial_cost

    // Missing in schema but previously added (Weight). Remove or keep? 
    // User schema did NOT show weight. Removing to prevent errors.

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Not in schema explicitly? Schema has created_at default current_timestamp. No updated_at? 
    // Schema: created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    // I will keep UpdatedAt unmapped or remove if strict. Let's remove UpdatedAt to be safe or ignore it.
}
