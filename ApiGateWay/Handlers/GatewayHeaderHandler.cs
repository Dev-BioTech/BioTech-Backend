using System.Security.Claims;

namespace ApiGateWay.Handlers;

public class GatewayHeaderHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GatewayHeaderHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext != null)
        {
            // Add Gateway Secret
            var gatewaySecret = _configuration["Gateway:Secret"] ?? Environment.GetEnvironmentVariable("GATEWAY_SECRET") ?? "secret123";
            request.Headers.TryAddWithoutValidation("X-Gateway-Secret", gatewaySecret);

            // Extract user claims from JWT and forward them as headers
            var user = httpContext.User;
            
            if (user?.Identity?.IsAuthenticated == true)
            {
                // User ID
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? user.FindFirst("sub")?.Value 
                          ?? user.FindFirst("userId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    request.Headers.TryAddWithoutValidation("X-User-Id", userId);
                }

                // Email
                var email = user.FindFirst(ClaimTypes.Email)?.Value 
                         ?? user.FindFirst("email")?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    request.Headers.TryAddWithoutValidation("X-User-Email", email);
                }

                // Username
                var username = user.FindFirst(ClaimTypes.Name)?.Value 
                            ?? user.FindFirst("name")?.Value 
                            ?? user.FindFirst("username")?.Value;
                if (!string.IsNullOrEmpty(username))
                {
                    request.Headers.TryAddWithoutValidation("X-User-Name", username);
                }

                // Roles
                var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                if (!roles.Any())
                {
                    roles = user.FindAll("role").Select(c => c.Value).ToList();
                }
                if (roles.Any())
                {
                    request.Headers.TryAddWithoutValidation("X-User-Roles", string.Join(",", roles));
                }

                // Farm ID (business-specific claim)
                var farmId = user.FindFirst("farmId")?.Value 
                          ?? user.FindFirst("FarmId")?.Value;
                if (!string.IsNullOrEmpty(farmId))
                {
                    request.Headers.TryAddWithoutValidation("X-Farm-Id", farmId);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
