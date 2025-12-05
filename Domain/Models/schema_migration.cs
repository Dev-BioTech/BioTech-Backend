using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Manages updates to the auth system.
/// </summary>
[Table("schema_migrations", Schema = "auth")]
public partial class schema_migration
{
    [Key]
    [StringLength(255)]
    public string version { get; set; } = null!;
}
