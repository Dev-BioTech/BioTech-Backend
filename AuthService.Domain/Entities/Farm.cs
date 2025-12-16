using System;
using System.Collections.Generic;

namespace AuthService.Domain.Entities;

public class Farm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;

    // Navigation properties
    public ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();
}
