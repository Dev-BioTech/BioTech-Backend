using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("schema_migrations", Schema = "realtime")]
public partial class schema_migration1
{
    [Key]
    public long version { get; set; }

    [Column(TypeName = "timestamp(0) without time zone")]
    public DateTime? inserted_at { get; set; }
}
