using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("withdrawal_periods")]
[Index("animal_id", "end_date", Name = "idx_withdrawals_date")]
public partial class WithdrawalPeriod
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("animal_id")]
    public long AnimalId { get; set; }

    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("withdrawal_days")]
    public int WithdrawalDays { get; set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; set; }

    [StringLength(20)]
    [Column("product_type")]
    public string? ProductType { get; set; }

    [StringLength(100)]
    [Column("reason")]
    public string? Reason { get; set; }

    [Column("active")]
    public bool? Active { get; set; }

    [ForeignKey("animal_id")]
    [InverseProperty("withdrawal_periods")]
    public virtual Animal Animal { get; set; } = null!;

    [ForeignKey("farm_id")]
    [InverseProperty("withdrawal_periods")]
    public virtual Farm Farm { get; set; } = null!;
}
