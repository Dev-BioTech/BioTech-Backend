using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("third_parties")]
[Index("farm_id", Name = "idx_third_parties_farm")]
[Index("farm_id", "identity_document", Name = "uk_third_party_doc_farm", IsUnique = true)]
public partial class ThirdParty
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [StringLength(150)]
    [Column("full_name")]
    public string FullName { get; set; } = null!;

    [StringLength(20)]
    [Column("identity_document")]
    public string IdentityDocument { get; set; } = null!;

    [StringLength(20)]
    [Column("phone")]
    public string? Phone { get; set; }

    [StringLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [Column("is_supplier")]
    public bool? IsSupplier { get; set; }

    [Column("is_customer")]
    public bool? IsCustomer { get; set; }

    [Column("is_employee")]
    public bool? IsEmployee { get; set; }

    [Column("is_veterinarian")]
    public bool? IsVeterinarian { get; set; }

    [StringLength(200)]
    [Column("address")]
    public string? Address { get; set; }

    [InverseProperty("ThirdParty")]
    public virtual ICollection<AnimalMovement> AnimalMovements { get; set; } = new List<AnimalMovement>();

    [InverseProperty("ThirdParty")]
    public virtual ICollection<CommercialTransaction> CommercialTransactions { get; set; } = new List<CommercialTransaction>();

    [ForeignKey("farm_id")]
    [InverseProperty("third_parties")]
    public virtual Farm Farm { get; set; } = null!;

    [InverseProperty("professional")]
    public virtual ICollection<HealthEvent> HealthEvents { get; set; } = new List<HealthEvent>();

    [InverseProperty("ThirdParty")]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}
