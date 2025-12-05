using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[PrimaryKey("bucket_id", "level", "name")]
[Table("prefixes", Schema = "storage")]
public partial class prefix
{
    [Key]
    public string bucket_id { get; set; } = null!;

    [Key]
    public string name { get; set; } = null!;

    [Key]
    public int level { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("prefixes")]
    public virtual bucket bucket { get; set; } = null!;
}
