using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Manages SAML Identity Provider connections.
/// </summary>
[Table("saml_providers", Schema = "auth")]
[Index("entity_id", Name = "saml_providers_entity_id_key", IsUnique = true)]
[Index("sso_provider_id", Name = "saml_providers_sso_provider_id_idx")]
public partial class saml_provider
{
    [Key]
    public Guid id { get; set; }

    public Guid sso_provider_id { get; set; }

    public string entity_id { get; set; } = null!;

    public string metadata_xml { get; set; } = null!;

    public string? metadata_url { get; set; }

    [Column(TypeName = "jsonb")]
    public string? attribute_mapping { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? name_id_format { get; set; }

    [ForeignKey("sso_provider_id")]
    [InverseProperty("saml_providers")]
    public virtual sso_provider sso_provider { get; set; } = null!;
}
