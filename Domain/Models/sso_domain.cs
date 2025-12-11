using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

/// <summary>
/// Auth: Manages SSO email address domain mapping to an SSO Identity Provider.
/// </summary>
[Table("sso_domains", Schema = "auth")]
[Index("sso_provider_id", Name = "sso_domains_sso_provider_id_idx")]
public partial class SsoDomain
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sso_provider_id")]
    public Guid SsoProviderId { get; set; }

    [Column("domain")]
    public string Domain { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("sso_provider_id")]
    [InverseProperty("sso_domains")]
    public virtual SsoProvider SsoProvider { get; set; } = null!;
}
