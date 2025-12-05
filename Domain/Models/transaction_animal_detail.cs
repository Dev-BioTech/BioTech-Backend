using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public partial class transaction_animal_detail
{
    [Key]
    public long id { get; set; }

    public long transaction_id { get; set; }

    public long animal_id { get; set; }

    [Precision(10, 2)]
    public decimal? price_per_kilo { get; set; }

    [Precision(6, 2)]
    public decimal? weight_at_negotiation { get; set; }

    [Precision(12, 2)]
    public decimal base_head_price { get; set; }

    [Precision(12, 2)]
    public decimal? commission_cost { get; set; }

    [Precision(12, 2)]
    public decimal? transport_cost { get; set; }

    [Precision(12, 2)]
    public decimal? final_line_value { get; set; }

    public long? animal_movement_id { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual animal animal { get; set; } = null!;

    [ForeignKey("animal_movement_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual animal_movement? animal_movement { get; set; }

    [ForeignKey("transaction_id")]
    [InverseProperty("transaction_animal_details")]
    public virtual commercial_transaction transaction { get; set; } = null!;
}
