using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FeedingService.Presentation.Authorization.Attributes;

/// <summary>
/// Custom authorization attribute to validate user roles from Gateway headers
/// </summary>
public class RequireRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public RequireRoleAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                message = "User is not authenticated"
            });
            return;
        }

        var hasRole = _roles.Any(role => user.IsInRole(role));

        if (!hasRole)
        {
            context.Result = new ForbidResult();
        }
    }
}