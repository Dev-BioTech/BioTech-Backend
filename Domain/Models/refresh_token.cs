using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

/// <summary>
/// Auth: Store of tokens used to refresh JWT tokens once they expire.
/// </summary>
[Table("refresh_tokens", Schema = "auth")]
[Index("instance_id", Name = "refresh_tokens_instance_id_idx")]
[Index("instance_id", "user_id", Name = "refresh_tokens_instance_id_user_id_idx")]
[Index("parent", Name = "refresh_tokens_parent_idx")]
[Index("session_id", "revoked", Name = "refresh_tokens_session_id_revoked_idx")]
[Index("token", Name = "refresh_tokens_token_unique", IsUnique = true)]
[Index("updated_at", Name = "refresh_tokens_updated_at_idx", AllDescending = true)]
public partial class RefreshToken
{
    [Column("instance_id")]
    public Guid? InstanceId { get; set; }

    [Key]
    [Column("id")]
    public long Id { get; set; }

    [StringLength(255)]
    [Column("token")]
    public string? Token { get; set; }

    [StringLength(255)]
    [Column("user_id")]
    public string? UserId { get; set; }

    [Column("revoked")]
    public bool? Revoked { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [StringLength(255)]
    [Column("parent")]
    public string? Parent { get; set; }

    [Column("session_id")]
    public Guid? SessionId { get; set; }

    [ForeignKey("session_id")]
    [InverseProperty("refresh_tokens")]
    public virtual Session? Session { get; set; }
}
