using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("roles")]
[Index("name", Name = "roles_name_key", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();

    [ForeignKey("role_id")]
    [InverseProperty("roles")]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
