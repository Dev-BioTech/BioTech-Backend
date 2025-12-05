using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("buckets", Schema = "storage")]
[Index("name", Name = "bname", IsUnique = true)]
public partial class bucket
{
    [Key]
    public string id { get; set; } = null!;

    public string name { get; set; } = null!;

    /// <summary>
    /// Field is deprecated, use owner_id instead
    /// </summary>
    public Guid? owner { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    [Column("public")]
    public bool? _public { get; set; }

    public bool? avif_autodetection { get; set; }

    public long? file_size_limit { get; set; }

    public List<string>? allowed_mime_types { get; set; }

    public string? owner_id { get; set; }

    [InverseProperty("bucket")]
    public virtual IEnumerable<objects>? objects { get; set; } = new List<objects>();

    [InverseProperty("bucket")]
    public virtual ICollection<prefix> prefixes { get; set; } = new List<prefix>();

    [InverseProperty("bucket")]
    public virtual ICollection<s3_multipart_upload> s3_multipart_uploads { get; set; } = new List<s3_multipart_upload>();

    [InverseProperty("bucket")]
    public virtual ICollection<s3_multipart_uploads_part> s3_multipart_uploads_parts { get; set; } = new List<s3_multipart_uploads_part>();
}
