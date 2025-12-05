using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Stores identities associated to a user.
/// </summary>
[Table("identities", Schema = "auth")]
[Index("provider_id", "provider", Name = "identities_provider_id_provider_unique", IsUnique = true)]
[Index("user_id", Name = "identities_user_id_idx")]
public partial class identity
{
    public string provider_id { get; set; } = null!;

    public Guid user_id { get; set; }

    [Column(TypeName = "jsonb")]
    public string identity_data { get; set; } = null!;

    public string provider { get; set; } = null!;

    public DateTime? last_sign_in_at { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    /// <summary>
    /// Auth: Email is a generated column that references the optional email property in the identity_data
    /// </summary>
    public string? email { get; set; }

    [Key]
    public Guid id { get; set; }

    [ForeignKey("user_id")]
    [InverseProperty("identities")]
    public virtual user1 user { get; set; } = null!;
}
