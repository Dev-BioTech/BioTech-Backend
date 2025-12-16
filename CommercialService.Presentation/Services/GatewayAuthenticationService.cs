using System.Security.Claims;

namespace CommercialService.Presentation.Services;

/// <summary>
/// Service to extract user information from Gateway headers
/// </summary>
public class GatewayAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GatewayAuthenticationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User
            .FindFirst(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "userId")?.Value;

        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    public string? GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? GetUserName()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Name)?.Value;
    }

    public List<string> GetUserRoles()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
    }

    public int? GetFarmId()
    {
        var farmIdClaim = _httpContextAccessor.HttpContext?.User
            .FindFirst("farmId")?.Value;

        return int.TryParse(farmIdClaim, out var farmId) ? farmId : null;
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
