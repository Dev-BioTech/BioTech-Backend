using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Table("oauth_clients", Schema = "auth")]
[Index("deleted_at", Name = "oauth_clients_deleted_at_idx")]
public partial class OauthClient
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("client_secret_hash")]
    public string? ClientSecretHash { get; set; }

    [Column("redirect_uris")]
    public string RedirectUris { get; set; } = null!;

    [Column("grant_types")]
    public string GrantTypes { get; set; } = null!;

    [Column("client_name")]
    public string? ClientName { get; set; }

    [Column("client_uri")]
    public string? ClientUri { get; set; }

    [Column("logo_uri")]
    public string? LogoUri { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("client")]
    public virtual ICollection<OauthAuthorization> OauthAuthorizations { get; set; } = new List<OauthAuthorization>();

    [InverseProperty("client")]
    public virtual ICollection<OauthConsent> OauthConsents { get; set; } = new List<OauthConsent>();

    [InverseProperty("OauthClient")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
