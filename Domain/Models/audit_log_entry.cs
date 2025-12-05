using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// Auth: Audit trail for user actions.
/// </summary>
[Table("audit_log_entries", Schema = "auth")]
[Index("instance_id", Name = "audit_logs_instance_id_idx")]
public partial class audit_log_entry
{
    public Guid? instance_id { get; set; }

    [Key]
    public Guid id { get; set; }

    [Column(TypeName = "json")]
    public string? payload { get; set; }

    public DateTime? created_at { get; set; }

    [StringLength(64)]
    public string ip_address { get; set; } = null!;
}
