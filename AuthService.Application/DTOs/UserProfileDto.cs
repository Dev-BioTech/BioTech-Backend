using System.Collections.Generic;

namespace AuthService.Application.DTOs;

public class UserProfileDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; }
    public List<string> Roles { get; set; } = new();
}
