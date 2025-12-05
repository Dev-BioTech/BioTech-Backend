using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("oauth_authorizations", Schema = "auth")]
[Index("authorization_code", Name = "oauth_authorizations_authorization_code_key", IsUnique = true)]
[Index("authorization_id", Name = "oauth_authorizations_authorization_id_key", IsUnique = true)]
public partial class oauth_authorization
{
    [Key]
    public Guid id { get; set; }

    public string authorization_id { get; set; } = null!;

    public Guid client_id { get; set; }

    public Guid? user_id { get; set; }

    public string redirect_uri { get; set; } = null!;

    public string scope { get; set; } = null!;

    public string? state { get; set; }

    public string? resource { get; set; }

    public string? code_challenge { get; set; }

    public string? authorization_code { get; set; }

    public DateTime created_at { get; set; }

    public DateTime expires_at { get; set; }

    public DateTime? approved_at { get; set; }

    public string? nonce { get; set; }

    [ForeignKey("client_id")]
    [InverseProperty("oauth_authorizations")]
    public virtual oauth_client client { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("oauth_authorizations")]
    public virtual user1? user { get; set; }
}
