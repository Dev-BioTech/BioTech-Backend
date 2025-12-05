using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Contains SAML Relay State information for each Service Provider initiated login.
/// </summary>
[Table("saml_relay_states", Schema = "auth")]
[Index("created_at", Name = "saml_relay_states_created_at_idx", AllDescending = true)]
[Index("for_email", Name = "saml_relay_states_for_email_idx")]
[Index("sso_provider_id", Name = "saml_relay_states_sso_provider_id_idx")]
public partial class saml_relay_state
{
    [Key]
    public Guid id { get; set; }

    public Guid sso_provider_id { get; set; }

    public string request_id { get; set; } = null!;

    public string? for_email { get; set; }

    public string? redirect_to { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public Guid? flow_state_id { get; set; }

    [ForeignKey("flow_state_id")]
    [InverseProperty("saml_relay_states")]
    public virtual flow_state? flow_state { get; set; }

    [ForeignKey("sso_provider_id")]
    [InverseProperty("saml_relay_states")]
    public virtual sso_provider sso_provider { get; set; } = null!;
}
