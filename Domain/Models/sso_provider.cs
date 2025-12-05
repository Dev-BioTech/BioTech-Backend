using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Manages SSO identity provider information; see saml_providers for SAML.
/// </summary>
[Table("sso_providers", Schema = "auth")]
public partial class sso_provider
{
    [Key]
    public Guid id { get; set; }

    /// <summary>
    /// Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code.
    /// </summary>
    public string? resource_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public bool? disabled { get; set; }

    [InverseProperty("sso_provider")]
    public virtual ICollection<saml_provider> saml_providers { get; set; } = new List<saml_provider>();

    [InverseProperty("sso_provider")]
    public virtual ICollection<saml_relay_state> saml_relay_states { get; set; } = new List<saml_relay_state>();

    [InverseProperty("sso_provider")]
    public virtual ICollection<sso_domain> sso_domains { get; set; } = new List<sso_domain>();
}
