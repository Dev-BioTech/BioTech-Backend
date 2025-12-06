using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Index("animal_id", "movement_date", Name = "idx_animal_movements_date")]
public partial class AnimalMovement
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("animal_id")]
    public long AnimalId { get; set; }

    [Column("movement_type_id")]
    public int MovementTypeId { get; set; }

    [Column("movement_date")]
    public DateOnly MovementDate { get; set; }

    [Column("previous_batch_id")]
    public int? PreviousBatchId { get; set; }

    [Column("new_batch_id")]
    public int? NewBatchId { get; set; }

    [Column("previous_paddock_id")]
    public int? PreviousPaddockId { get; set; }

    [Column("new_paddock_id")]
    public int? NewPaddockId { get; set; }

    [Column("third_party_id")]
    public long? ThirdPartyId { get; set; }

    [Precision(12, 2)]
    [Column("transaction_value")]
    public decimal? TransactionValue { get; set; }

    [Precision(6, 2)]
    [Column("weight_at_movement")]
    public decimal? WeightAtMovement { get; set; }

    [Column("observations")]
    public string? Observations { get; set; }

    [Column("registered_by")]
    public int? RegisteredBy { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("animal_movements")]
    public virtual Animal Animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("animal_movements")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("movement_type_id")]
    [InverseProperty("animal_movements")]
    public virtual MovementType MovementType { get; set; } = null!;

    [ForeignKey("new_batch_id")]
    [InverseProperty("animal_movementnew_batches")]
    public virtual Batch? NewBatch { get; set; }

    [ForeignKey("new_paddock_id")]
    [InverseProperty("animal_movementnew_paddocks")]
    public virtual Paddock? NewPaddock { get; set; }

    [ForeignKey("previous_batch_id")]
    [InverseProperty("animal_movementprevious_batches")]
    public virtual Batch? PreviousBatch { get; set; }

    [ForeignKey("previous_paddock_id")]
    [InverseProperty("animal_movementprevious_paddocks")]
    public virtual Paddock? PreviousPaddock { get; set; }

    [ForeignKey("registered_by")]
    [InverseProperty("animal_movements")]
    public virtual User? RegisteredBynavigation { get; set; }

    [ForeignKey("third_party_id")]
    [InverseProperty("animal_movements")]
    public virtual ThirdParty? ThirdParty { get; set; }

    [InverseProperty("animal_movement")]
    public virtual ICollection<TransactionAnimalDetail> TransactionAnimalDetails { get; set; } = new List<TransactionAnimalDetail>();
}
