using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Manages SSO email address domain mapping to an SSO Identity Provider.
/// </summary>
[Table("sso_domains", Schema = "auth")]
[Index("sso_provider_id", Name = "sso_domains_sso_provider_id_idx")]
public partial class sso_domain
{
    [Key]
    public Guid id { get; set; }

    public Guid sso_provider_id { get; set; }

    public string domain { get; set; } = null!;

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    [ForeignKey("sso_provider_id")]
    [InverseProperty("sso_domains")]
    public virtual sso_provider sso_provider { get; set; } = null!;
}
