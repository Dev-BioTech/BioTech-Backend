using System;

namespace AuthService.Application.DTOs;

public class CreateTenantDto
{
    public string Name { get; set; } = string.Empty;
}

public class TenantResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}
