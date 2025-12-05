using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// auth: stores authenticator method reference claims for multi factor authentication
/// </summary>
[Table("mfa_amr_claims", Schema = "auth")]
[Index("session_id", "authentication_method", Name = "mfa_amr_claims_session_id_authentication_method_pkey", IsUnique = true)]
public partial class mfa_amr_claim
{
    public Guid session_id { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }

    public string authentication_method { get; set; } = null!;

    [Key]
    public Guid id { get; set; }

    [ForeignKey("session_id")]
    [InverseProperty("mfa_amr_claims")]
    public virtual session session { get; set; } = null!;
}
