using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

/// <summary>
/// Auth: Stores User login data within a secure schema.
/// </summary>
[Table("users", Schema = "auth")]
[Index("instance_id", Name = "users_instance_id_idx")]
[Index("is_anonymous", Name = "users_is_anonymous_idx")]
[Index("phone", Name = "users_phone_key", IsUnique = true)]
public partial class User1
{
    [Column("instance_id")]
    public Guid? InstanceId { get; set; }

    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [StringLength(255)]
    [Column("aud")]
    public string? Aud { get; set; }

    [StringLength(255)]
    [Column("Role")]
    public string? Role { get; set; }

    [StringLength(255)]
    [Column("email")]
    public string? Email { get; set; }

    [StringLength(255)]
    [Column("encrypted_password")]
    public string? EncryptedPassword { get; set; }

    [Column("email_confirmed_at")]
    public DateTime? EmailConfirmedAt { get; set; }

    [Column("invited_at")]
    public DateTime? InvitedAt { get; set; }

    [StringLength(255)]
    [Column("confirmation_token")]
    public string? ConfirmationToken { get; set; }

    [Column("confirmation_sent_at")]
    public DateTime? ConfirmationSentAt { get; set; }

    [StringLength(255)]
    [Column("recovery_token")]
    public string? RecoveryToken { get; set; }

    [Column("recovery_sent_at")]
    public DateTime? RecoverySentAt { get; set; }

    [StringLength(255)]
    [Column("email_change_token_new")]
    public string? EmailChangeTokenNew { get; set; }

    [StringLength(255)]
    [Column("email_change")]
    public string? EmailChange { get; set; }

    [Column("email_change_sent_at")]
    public DateTime? EmailChangeSentAt { get; set; }

    [Column("last_sign_in_at")]
    public DateTime? LastSignInAt { get; set; }

    [Column(TypeName = "jsonb")]
    public string? RawAppMetaData { get; set; }

    [Column(TypeName = "jsonb")]
    public string? RawUserMetaData { get; set; }

    [Column("is_super_admin")]
    public bool? IsSuperAdmin { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("phone_confirmed_at")]
    public DateTime? PhoneConfirmedAt { get; set; }

    [Column("phone_change")]
    public string? PhoneChange { get; set; }

    [StringLength(255)]
    [Column("phone_change_token")]
    public string? PhoneChangeToken { get; set; }

    [Column("phone_change_sent_at")]
    public DateTime? PhoneChangeSentAt { get; set; }

    [Column("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }

    [StringLength(255)]
    [Column("email_change_token_current")]
    public string? EmailChangeTokenCurrent { get; set; }

    [Column("email_change_confirm_status")]
    public short? EmailChangeConfirmStatus { get; set; }

    [Column("banned_until")]
    public DateTime? BannedUntil { get; set; }

    [StringLength(255)]
    [Column("reauthentication_token")]
    public string? ReauthenticationToken { get; set; }

    [Column("reauthentication_sent_at")]
    public DateTime? ReauthenticationSentAt { get; set; }

    /// <summary>
    /// Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails.
    /// </summary>
    [Column("is_sso_user")]
    public bool IsSsoUser { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [Column("is_anonymous")]
    public bool IsAnonymous { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Identity> Identities { get; set; } = new List<Identity>();

    [InverseProperty("User")]
    public virtual ICollection<MfaFactor> MfaFactors { get; set; } = new List<MfaFactor>();

    [InverseProperty("User")]
    public virtual ICollection<OauthAuthorization> OauthAuthorizations { get; set; } = new List<OauthAuthorization>();

    [InverseProperty("User")]
    public virtual ICollection<OauthConsent> OauthConsents { get; set; } = new List<OauthConsent>();

    [InverseProperty("User")]
    public virtual ICollection<OneTimeToken> OneTimeTokens { get; set; } = new List<OneTimeToken>();

    [InverseProperty("User")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
