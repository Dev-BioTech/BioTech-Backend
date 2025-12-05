using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// auth: stores metadata about challenge requests made
/// </summary>
[Table("mfa_challenges", Schema = "auth")]
[Index("created_at", Name = "mfa_challenge_created_at_idx", AllDescending = true)]
public partial class mfa_challenge
{
    [Key]
    public Guid id { get; set; }

    public Guid factor_id { get; set; }

    public DateTime created_at { get; set; }

    public DateTime? verified_at { get; set; }

    public IPAddress ip_address { get; set; } = null!;

    public string? otp_code { get; set; }

    [Column(TypeName = "jsonb")]
    public string? web_authn_session_data { get; set; }

    [ForeignKey("factor_id")]
    [InverseProperty("mfa_challenges")]
    public virtual mfa_factor factor { get; set; } = null!;
}
