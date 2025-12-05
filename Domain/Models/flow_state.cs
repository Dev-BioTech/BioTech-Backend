using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// stores metadata for pkce logins
/// </summary>
[Table("flow_state", Schema = "auth")]
[Index("created_at", Name = "flow_state_created_at_idx", AllDescending = true)]
[Index("auth_code", Name = "idx_auth_code")]
[Index("user_id", "authentication_method", Name = "idx_user_id_auth_method")]
public partial class flow_state
{
    [Key]
    public Guid id { get; set; }

    public Guid? user_id { get; set; }

    public string auth_code { get; set; } = null!;

    public string code_challenge { get; set; } = null!;

    public string provider_type { get; set; } = null!;

    public string? provider_access_token { get; set; }

    public string? provider_refresh_token { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string authentication_method { get; set; } = null!;

    public DateTime? auth_code_issued_at { get; set; }

    [InverseProperty("flow_state")]
    public virtual ICollection<saml_relay_state> saml_relay_states { get; set; } = new List<saml_relay_state>();
}
