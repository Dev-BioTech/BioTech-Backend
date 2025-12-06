using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("migrations", Schema = "storage")]
[Index("name", Name = "migrations_name_key", IsUnique = true)]
public partial class Migration
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = null!;

    [StringLength(40)]
    [Column("hash")]
    public string Hash { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ExecutedAt { get; set; }
}
