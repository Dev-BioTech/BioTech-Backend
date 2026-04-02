using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SalesService.Presentation.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "userId")?.Value;
            if (int.TryParse(userIdClaim, out var userId)) return userId;

            // Fallback: Check for header from Gateway
            var headerUserId = _httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].ToString();
            return int.TryParse(headerUserId, out var hUserId) ? hUserId : null;
        }
    }

    public string? Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Email)?.Value;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
