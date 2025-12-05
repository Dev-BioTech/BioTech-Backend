using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Index("name", Name = "roles_name_key", IsUnique = true)]
public partial class role
{
    [Key]
    public int id { get; set; }

    [StringLength(50)]
    public string name { get; set; } = null!;

    [StringLength(200)]
    public string? description { get; set; }

    [InverseProperty("role")]
    public virtual ICollection<user_farm_role> user_farm_roles { get; set; } = new List<user_farm_role>();

    [ForeignKey("role_id")]
    [InverseProperty("roles")]
    public virtual ICollection<permission> permissions { get; set; } = new List<permission>();
}
