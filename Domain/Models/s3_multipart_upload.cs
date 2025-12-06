using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("s3_multipart_uploads", Schema = "storage")]
public partial class S3MultipartUpload
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("in_progress_size")]
    public long InProgressSize { get; set; }

    [Column("upload_signature")]
    public string UploadSignature { get; set; } = null!;

    [Column("bucket_id")]
    public string BucketId { get; set; } = null!;

    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("version")]
    public string Version { get; set; } = null!;

    [Column("owner_id")]
    public string? OwnerId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? UserMetadata { get; set; }

    [ForeignKey("bucket_id")]
    [InverseProperty("s3_multipart_uploads")]
    public virtual Bucket Bucket { get; set; } = null!;

    [InverseProperty("upload")]
    public virtual ICollection<S3MultipartUploadsPart> S3MultipartUploadsParts { get; set; } = new List<S3MultipartUploadsPart>();
}
