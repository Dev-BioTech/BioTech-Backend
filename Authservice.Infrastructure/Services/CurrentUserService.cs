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
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                .FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type == "userId" || c.Type == "sub")?.Value;

            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
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
