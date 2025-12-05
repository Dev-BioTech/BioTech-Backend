using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

[Table("subscription", Schema = "realtime")]
public partial class subscription
{
    [Key]
    public long id { get; set; }

    public Guid subscription_id { get; set; }

    [Column(TypeName = "jsonb")]
    public string claims { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime created_at { get; set; }
}
