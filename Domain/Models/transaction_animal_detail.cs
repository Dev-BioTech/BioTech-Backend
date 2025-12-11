using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("transaction_animal_details")]
public partial class TransactionAnimalDetail
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("transaction_id")]
    public long TransactionId { get; set; }

    [Column("animal_id")]
    public long AnimalId { get; set; }

    [Precision(10, 2)]
    [Column("price_per_kilo")]
    public decimal? PricePerKilo { get; set; }

    [Precision(6, 2)]
    [Column("weight_at_negotiation")]
    public decimal? WeightAtNegotiation { get; set; }

    [Precision(12, 2)]
    [Column("base_head_price")]
    public decimal BaseHeadPrice { get; set; }

    [Precision(12, 2)]
    [Column("commission_cost")]
    public decimal? CommissionCost { get; set; }

    [Precision(12, 2)]
    [Column("transport_cost")]
    public decimal? TransportCost { get; set; }

    [Precision(12, 2)]
    [Column("final_line_value")]
    public decimal? FinalLineValue { get; set; }

    [Column("animal_movement_id")]
    public long? AnimalMovementId { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual Animal Animal { get; set; } = null!;

    [ForeignKey("animal_movement_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual AnimalMovement? AnimalMovement { get; set; }

    [ForeignKey("transaction_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual CommercialTransaction Transaction { get; set; } = null!;
}
