using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

/// <summary>
/// Auth: Manages SAML Identity Provider connections.
/// </summary>
[Table("saml_providers", Schema = "auth")]
[Index("entity_id", Name = "saml_providers_entity_id_key", IsUnique = true)]
[Index("sso_provider_id", Name = "saml_providers_sso_provider_id_idx")]
public partial class SamlProvider
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sso_provider_id")]
    public Guid SsoProviderId { get; set; }

    [Column("entity_id")]
    public string EntityId { get; set; } = null!;

    [Column("metadata_xml")]
    public string MetadataXml { get; set; } = null!;

    [Column("metadata_url")]
    public string? MetadataUrl { get; set; }

    [Column(TypeName = "jsonb")]
    public string? AttributeMapping { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("name_id_format")]
    public string? NameIdFormat { get; set; }

    [ForeignKey("sso_provider_id")]
    [InverseProperty("saml_providers")]
    public virtual SsoProvider SsoProvider { get; set; } = null!;
}
