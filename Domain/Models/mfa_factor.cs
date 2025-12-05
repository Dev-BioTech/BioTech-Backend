using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// auth: stores metadata about factors
/// </summary>
[Table("mfa_factors", Schema = "auth")]
[Index("user_id", "created_at", Name = "factor_id_created_at_idx")]
[Index("last_challenged_at", Name = "mfa_factors_last_challenged_at_key", IsUnique = true)]
[Index("user_id", Name = "mfa_factors_user_id_idx")]
[Index("user_id", "phone", Name = "unique_phone_factor_per_user", IsUnique = true)]
public partial class mfa_factor
{
    [Key]
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public string? friendly_name { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public string? secret { get; set; }

    public string? phone { get; set; }

    public DateTime? last_challenged_at { get; set; }

    [Column(TypeName = "jsonb")]
    public string? web_authn_credential { get; set; }

    public Guid? web_authn_aaguid { get; set; }

    /// <summary>
    /// Stores the latest WebAuthn challenge data including attestation/assertion for customer verification
    /// </summary>
    [Column(TypeName = "jsonb")]
    public string? last_webauthn_challenge_data { get; set; }

    [InverseProperty("factor")]
    public virtual ICollection<mfa_challenge> mfa_challenges { get; set; } = new List<mfa_challenge>();

    [ForeignKey("user_id")]
    [InverseProperty("mfa_factors")]
    public virtual user1 user { get; set; } = null!;
}
