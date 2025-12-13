using System.Collections.Generic;

namespace AuthService.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();
}
