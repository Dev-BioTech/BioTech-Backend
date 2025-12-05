using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("oauth_consents", Schema = "auth")]
[Index("user_id", "client_id", Name = "oauth_consents_user_client_unique", IsUnique = true)]
[Index("user_id", "granted_at", Name = "oauth_consents_user_order_idx", IsDescending = new[] { false, true })]
public partial class oauth_consent
{
    [Key]
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public Guid client_id { get; set; }

    public string scopes { get; set; } = null!;

    public DateTime granted_at { get; set; }

    public DateTime? revoked_at { get; set; }

    [ForeignKey("client_id")]
    [InverseProperty("oauth_consents")]
    public virtual oauth_client client { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("oauth_consents")]
    public virtual user1 user { get; set; } = null!;
}
