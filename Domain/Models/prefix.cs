using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[PrimaryKey("bucket_id", "level", "name")]
[Table("prefixes", Schema = "storage")]
public partial class Prefix
{
    [Key]
    [Column("bucket_id")]
    public string BucketId { get; set; } = null!;

    [Key]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Key]
    [Column("level")]
    public int Level { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("prefixes")]
    public virtual Bucket Bucket { get; set; } = null!;
}
