using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("farm_id", Name = "idx_third_parties_farm")]
[Index("farm_id", "identity_document", Name = "uk_third_party_doc_farm", IsUnique = true)]
public partial class third_party
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    [StringLength(150)]
    public string full_name { get; set; } = null!;

    [StringLength(20)]
    public string identity_document { get; set; } = null!;

    [StringLength(20)]
    public string? phone { get; set; }

    [StringLength(100)]
    public string? email { get; set; }

    public bool? is_supplier { get; set; }

    public bool? is_customer { get; set; }

    public bool? is_employee { get; set; }

    public bool? is_veterinarian { get; set; }

    [StringLength(200)]
    public string? address { get; set; }

    [InverseProperty("third_party")]
    public virtual ICollection<animal_movement> animal_movements { get; set; } = new List<animal_movement>();

    [InverseProperty("third_party")]
    public virtual ICollection<commercial_transaction> commercial_transactions { get; set; } = new List<commercial_transaction>();

    [ForeignKey("farm_id")]
    [InverseProperty("third_parties")]
    public virtual farm farm { get; set; } = null!;

    [InverseProperty("professional")]
    public virtual ICollection<health_event> health_events { get; set; } = new List<health_event>();

    [InverseProperty("third_party")]
    public virtual ICollection<inventory_movement> inventory_movements { get; set; } = new List<inventory_movement>();
}
