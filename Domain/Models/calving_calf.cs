using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[PrimaryKey("calving_id", "calf_id")]
public partial class calving_calf
{
    [Key]
    public long calving_id { get; set; }

    [Key]
    public long calf_id { get; set; }

    [Precision(5, 2)]
    public decimal? birth_weight { get; set; }

    [StringLength(20)]
    public string? birth_status { get; set; }

    [ForeignKey("calf_id")]
    [InverseProperty("calving_calves")]
    public virtual animal calf { get; set; } = null!;

    [ForeignKey("calving_id")]
    [InverseProperty("calving_calves")]
    public virtual calving calving { get; set; } = null!;
}
