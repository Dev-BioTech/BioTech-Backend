using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Stores session data associated to a user.
/// </summary>
[Table("sessions", Schema = "auth")]
[Index("not_after", Name = "sessions_not_after_idx", AllDescending = true)]
[Index("oauth_client_id", Name = "sessions_oauth_client_id_idx")]
[Index("user_id", Name = "sessions_user_id_idx")]
[Index("user_id", "created_at", Name = "user_id_created_at_idx")]
public partial class session
{
    [Key]
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public Guid? factor_id { get; set; }

    /// <summary>
    /// Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired.
    /// </summary>
    public DateTime? not_after { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? refreshed_at { get; set; }

    public string? user_agent { get; set; }

    public IPAddress? ip { get; set; }

    public string? tag { get; set; }

    public Guid? oauth_client_id { get; set; }

    /// <summary>
    /// Holds a HMAC-SHA256 key used to sign refresh tokens for this session.
    /// </summary>
    public string? refresh_token_hmac_key { get; set; }

    /// <summary>
    /// Holds the ID (counter) of the last issued refresh token.
    /// </summary>
    public long? refresh_token_counter { get; set; }

    public string? scopes { get; set; }

    [InverseProperty("session")]
    public virtual ICollection<mfa_amr_claim> mfa_amr_claims { get; set; } = new List<mfa_amr_claim>();

    [ForeignKey("oauth_client_id")]
    [InverseProperty("sessions")]
    public virtual oauth_client? oauth_client { get; set; }

    [InverseProperty("session")]
    public virtual ICollection<refresh_token> refresh_tokens { get; set; } = new List<refresh_token>();

    [ForeignKey("user_id")]
    [InverseProperty("sessions")]
    public virtual user1 user { get; set; } = null!;
}
