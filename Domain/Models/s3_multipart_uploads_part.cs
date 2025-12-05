using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("s3_multipart_uploads_parts", Schema = "storage")]
public partial class s3_multipart_uploads_part
{
    [Key]
    public Guid id { get; set; }

    public string upload_id { get; set; } = null!;

    public long size { get; set; }

    public int part_number { get; set; }

    public string bucket_id { get; set; } = null!;

    public string key { get; set; } = null!;

    public string etag { get; set; } = null!;

    public string? owner_id { get; set; }

    public string version { get; set; } = null!;

    public DateTime created_at { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("s3_multipart_uploads_parts")]
    public virtual bucket bucket { get; set; } = null!;

    [ForeignKey("upload_id")]
    [InverseProperty("s3_multipart_uploads_parts")]
    public virtual s3_multipart_upload upload { get; set; } = null!;
}
