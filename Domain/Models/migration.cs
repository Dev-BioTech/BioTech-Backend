using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("migrations", Schema = "storage")]
[Index("name", Name = "migrations_name_key", IsUnique = true)]
public partial class migration
{
    [Key]
    public int id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(40)]
    public string hash { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? executed_at { get; set; }
}
