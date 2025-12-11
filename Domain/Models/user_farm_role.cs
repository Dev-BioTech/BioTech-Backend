using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("user_farm_role")]
[Index("user_id", "farm_id", Name = "uk_user_farm", IsUnique = true)]
public partial class UserFarmRole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("farm_id")]
    public int FarmId { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("user_farm_roles")]
    public virtual Farm Farm { get; set; } = null!;

    [ForeignKey("role_id")]
    [InverseProperty("user_farm_roles")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("user_farm_roles")]
    public virtual User User { get; set; } = null!;
}
