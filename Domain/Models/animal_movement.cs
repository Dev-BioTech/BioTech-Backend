using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("animal_id", "movement_date", Name = "idx_animal_movements_date")]
public partial class animal_movement
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    public long animal_id { get; set; }

    public int movement_type_id { get; set; }

    public DateOnly movement_date { get; set; }

    public int? previous_batch_id { get; set; }

    public int? new_batch_id { get; set; }

    public int? previous_paddock_id { get; set; }

    public int? new_paddock_id { get; set; }

    public long? third_party_id { get; set; }

    [Precision(12, 2)]
    public decimal? transaction_value { get; set; }

    [Precision(6, 2)]
    public decimal? weight_at_movement { get; set; }

    public string? observations { get; set; }

    public int? registered_by { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("animal_movements")]
    public virtual animal animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("animal_movements")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("movement_type_id")]
    [InverseProperty("animal_movements")]
    public virtual movement_type movement_type { get; set; } = null!;

    [ForeignKey("new_batch_id")]
    [InverseProperty("animal_movementnew_batches")]
    public virtual batch? new_batch { get; set; }

    [ForeignKey("new_paddock_id")]
    [InverseProperty("animal_movementnew_paddocks")]
    public virtual paddock? new_paddock { get; set; }

    [ForeignKey("previous_batch_id")]
    [InverseProperty("animal_movementprevious_batches")]
    public virtual batch? previous_batch { get; set; }

    [ForeignKey("previous_paddock_id")]
    [InverseProperty("animal_movementprevious_paddocks")]
    public virtual paddock? previous_paddock { get; set; }

    [ForeignKey("registered_by")]
    [InverseProperty("animal_movements")]
    public virtual user? registered_byNavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("animal_movements")]
    public virtual third_party? third_party { get; set; }

    [InverseProperty("animal_movement")]
    public virtual ICollection<transaction_animal_detail> transaction_animal_details { get; set; } = new List<transaction_animal_detail>();
}
