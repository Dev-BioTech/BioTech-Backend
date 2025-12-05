using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("vector_indexes", Schema = "storage")]
public partial class vector_index
{
    [Key]
    public string id { get; set; } = null!;

    public string name { get; set; } = null!;

    public string bucket_id { get; set; } = null!;

    public string data_type { get; set; } = null!;

    public int dimension { get; set; }

    public string distance_metric { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public string? metadata_configuration { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("vector_indices")]
    public virtual buckets_vector bucket { get; set; } = null!;
}
