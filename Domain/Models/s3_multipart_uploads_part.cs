using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("s3_multipart_uploads_parts", Schema = "storage")]
public partial class S3MultipartUploadsPart
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("upload_id")]
    public string UploadId { get; set; } = null!;

    [Column("size")]
    public long Size { get; set; }

    [Column("part_number")]
    public int PartNumber { get; set; }

    [Column("bucket_id")]
    public string BucketId { get; set; } = null!;

    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("etag")]
    public string Etag { get; set; } = null!;

    [Column("owner_id")]
    public string? OwnerId { get; set; }

    [Column("version")]
    public string Version { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("s3_multipart_uploads_parts")]
    public virtual Bucket Bucket { get; set; } = null!;

    [ForeignKey("upload_id")]
    [InverseProperty("s3_multipart_uploads_parts")]
    public virtual S3MultipartUpload Upload { get; set; } = null!;
}
