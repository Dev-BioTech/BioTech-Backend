using AuthService.Application.Interfaces;

namespace AuthService.Application.Services;

/// <summary>
/// Simple implementation for testing - in production this would be properly configured
/// Following existing patterns in the codebase
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    // This would be populated from JWT claims in production
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public bool IsAuthenticated => UserId.HasValue;
}
