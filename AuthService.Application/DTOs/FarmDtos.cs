using System;

namespace AuthService.Application.DTOs;

public class CreateFarmDto
{
    public string Name { get; set; } = string.Empty;
    public int TenantId { get; set; }
    public string? Address { get; set; }
    public string? Location { get; set; }
}

public class FarmResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TenantId { get; set; }
    public string? Address { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; }
}
