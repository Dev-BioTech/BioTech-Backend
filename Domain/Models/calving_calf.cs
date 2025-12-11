using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("calving_calves")]
[PrimaryKey("calving_id", "calf_id")]
public partial class CalvingCalf
{
    [Key]
    [Column("calving_id")]
    public long CalvingId { get; set; }

    [Key]
    [Column("calf_id")]
    public long CalfId { get; set; }

    [Precision(5, 2)]
    [Column("birth_weight")]
    public decimal? BirthWeight { get; set; }

    [StringLength(20)]
    [Column("birth_status")]
    public string? BirthStatus { get; set; }

    [ForeignKey("calf_id")]
    [InverseProperty("calving_calves")]
    public virtual Animal Calf { get; set; } = null!;

    [ForeignKey("calving_id")]
    [InverseProperty("calving_calves")]
    public virtual Calving Calving { get; set; } = null!;
}
