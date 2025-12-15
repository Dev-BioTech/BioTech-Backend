using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs;

public class UpdateProfileDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [MinLength(6)]
    public string? Password { get; set; }
}
