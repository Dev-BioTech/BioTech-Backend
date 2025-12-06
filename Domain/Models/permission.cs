using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("permissions")]
[Index("code", Name = "permissions_code_key", IsUnique = true)]
public partial class Permission
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Column("code")]
    public string Code { get; set; } = null!;

    [StringLength(200)]
    [Column("description")]
    public string? Description { get; set; }

    [ForeignKey("permission_id")]
    [InverseProperty("permissions")]
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
