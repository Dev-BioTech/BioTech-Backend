using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

/// <summary>
/// auth: stores metadata about factors
/// </summary>
[Table("mfa_factors", Schema = "auth")]
[Index("user_id", "created_at", Name = "factor_id_created_at_idx")]
[Index("last_challenged_at", Name = "mfa_factors_last_challenged_at_key", IsUnique = true)]
[Index("user_id", Name = "mfa_factors_user_id_idx")]
[Index("user_id", "phone", Name = "unique_phone_factor_per_user", IsUnique = true)]
public partial class MfaFactor
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("friendly_name")]
    public string? FriendlyName { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("secret")]
    public string? Secret { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("last_challenged_at")]
    public DateTime? LastChallengedAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? WebAuthnCredential { get; set; }

    [Column("web_authn_aaguid")]
    public Guid? WebAuthnAaguid { get; set; }

    /// <summary>
    /// Stores the latest WebAuthn challenge data including attestation/assertion for customer verification
    /// </summary>
    [Column(TypeName = "jsonb")]
    public string? LastWebauthnChallengeData { get; set; }

    [InverseProperty("factor")]
    public virtual ICollection<MfaChallenge> MfaChallenges { get; set; } = new List<MfaChallenge>();

    [ForeignKey("user_id")]
    [InverseProperty("mfa_factors")]
    public virtual User1 User { get; set; } = null!;
}
