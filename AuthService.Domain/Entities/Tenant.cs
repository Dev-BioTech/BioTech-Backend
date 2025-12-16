using System;
using System.Collections.Generic;

namespace AuthService.Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    // public ICollection<Farm> Farms { get; set; } = new List<Farm>();
}
