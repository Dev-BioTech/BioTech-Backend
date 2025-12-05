using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("s3_multipart_uploads", Schema = "storage")]
public partial class s3_multipart_upload
{
    [Key]
    public string id { get; set; } = null!;

    public long in_progress_size { get; set; }

    public string upload_signature { get; set; } = null!;

    public string bucket_id { get; set; } = null!;

    public string key { get; set; } = null!;

    public string version { get; set; } = null!;

    public string? owner_id { get; set; }

    public DateTime created_at { get; set; }

    [Column(TypeName = "jsonb")]
    public string? user_metadata { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("s3_multipart_uploads")]
    public virtual bucket bucket { get; set; } = null!;

    [InverseProperty("upload")]
    public virtual ICollection<s3_multipart_uploads_part> s3_multipart_uploads_parts { get; set; } = new List<s3_multipart_uploads_part>();
}
