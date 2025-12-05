using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("user_farm_role")]
[Index("user_id", "farm_id", Name = "uk_user_farm", IsUnique = true)]
public partial class user_farm_role
{
    [Key]
    public int id { get; set; }

    public int user_id { get; set; }

    public int farm_id { get; set; }

    public int role_id { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("user_farm_roles")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("role_id")]
    [InverseProperty("user_farm_roles")]
    public virtual role role { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("user_farm_roles")]
    public virtual user user { get; set; } = null!;
}
