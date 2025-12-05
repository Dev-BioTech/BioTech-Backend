using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("buckets_vectors", Schema = "storage")]
public partial class buckets_vector
{
    [Key]
    public string id { get; set; } = null!;

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    [InverseProperty("bucket")]
    public virtual ICollection<vector_index> vector_indices { get; set; } = new List<vector_index>();
}
