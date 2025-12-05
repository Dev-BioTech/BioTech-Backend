using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

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
public partial class refresh_token
{
    public Guid? instance_id { get; set; }

    [Key]
    public long id { get; set; }

    [StringLength(255)]
    public string? token { get; set; }

    [StringLength(255)]
    public string? user_id { get; set; }

    public bool? revoked { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    [StringLength(255)]
    public string? parent { get; set; }

    public Guid? session_id { get; set; }

    [ForeignKey("session_id")]
    [InverseProperty("refresh_tokens")]
    public virtual session? session { get; set; }
}
