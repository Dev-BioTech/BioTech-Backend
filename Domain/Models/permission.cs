using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("code", Name = "permissions_code_key", IsUnique = true)]
public partial class permission
{
    [Key]
    public int id { get; set; }

    [StringLength(50)]
    public string code { get; set; } = null!;

    [StringLength(200)]
    public string? description { get; set; }

    [ForeignKey("permission_id")]
    [InverseProperty("permissions")]
    public virtual ICollection<role> roles { get; set; } = new List<role>();
}
