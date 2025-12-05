using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("oauth_clients", Schema = "auth")]
[Index("deleted_at", Name = "oauth_clients_deleted_at_idx")]
public partial class oauth_client
{
    [Key]
    public Guid id { get; set; }

    public string? client_secret_hash { get; set; }

    public string redirect_uris { get; set; } = null!;

    public string grant_types { get; set; } = null!;

    public string? client_name { get; set; }

    public string? client_uri { get; set; }

    public string? logo_uri { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public DateTime? deleted_at { get; set; }

    [InverseProperty("client")]
    public virtual ICollection<oauth_authorization> oauth_authorizations { get; set; } = new List<oauth_authorization>();

    [InverseProperty("client")]
    public virtual ICollection<oauth_consent> oauth_consents { get; set; } = new List<oauth_consent>();

    [InverseProperty("oauth_client")]
    public virtual ICollection<session> sessions { get; set; } = new List<session>();
}
