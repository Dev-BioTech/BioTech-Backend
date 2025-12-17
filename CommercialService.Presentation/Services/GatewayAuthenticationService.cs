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
        // 1. Get all authorized tokens from "farm_role" claim
        var farmRoleClaims = _httpContextAccessor.HttpContext?.User
            .FindAll("farm_role")
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        var authorizedFarmIds = new List<int>();

        foreach (var claim in farmRoleClaims)
        {
            // Format is "FarmId:RoleName"
            var parts = claim.Split(':');
            if (parts.Length > 0 && int.TryParse(parts[0], out int id))
            {
                authorizedFarmIds.Add(id);
            }
        }

        // 2. Check for X-Farm-Id header
        var headerValue = _httpContextAccessor.HttpContext?.Request.Headers["X-Farm-Id"].ToString();
        if (!string.IsNullOrEmpty(headerValue) && int.TryParse(headerValue, out int headerFarmId))
        {
            if (authorizedFarmIds.Contains(headerFarmId))
            {
                return headerFarmId;
            }
        }

        // 3. Fallback: Return the first authorized farm (useful for testing single-farm users)
        return authorizedFarmIds.FirstOrDefault();
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public bool HasAccessToFarm(int farmId)
    {
        var farmRoleClaims = _httpContextAccessor.HttpContext?.User
            .FindAll("farm_role")
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        foreach (var claim in farmRoleClaims)
        {
            var parts = claim.Split(':');
            if (parts.Length > 0 && int.TryParse(parts[0], out int id))
            {
                if (id == farmId) return true;
            }
        }
        return false;
    }
}
