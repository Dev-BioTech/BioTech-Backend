using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AuthService.Infrastructure.Services;

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
            var httpContext = _httpContextAccessor.HttpContext;
            // 1. Try standard claims
            var userIdClaim = httpContext?.User
                .FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type == "userId" || c.Type == "sub")?.Value;

            // 2. Fallback to Gateway Headers directly (Immune to middleware overwriting)
            if (string.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = httpContext?.Request.Headers["X-User-Id"].FirstOrDefault();
            }

            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Email
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            // 1. Try standard claims
            var email = httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

            // 2. Fallback to Gateway Headers
            if (string.IsNullOrEmpty(email))
            {
                email = httpContext?.Request.Headers["X-User-Email"].FirstOrDefault();
            }

            return email;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return (httpContext?.User.Identity?.IsAuthenticated ?? false) || 
                   httpContext?.Request.Headers.ContainsKey("X-User-Id") == true;
        }
    }
}
