using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Manages users across multiple sites.
/// </summary>
[Table("instances", Schema = "auth")]
public partial class instance
{
    [Key]
    public Guid id { get; set; }

    public Guid? uuid { get; set; }

    public string? raw_base_config { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }
}
