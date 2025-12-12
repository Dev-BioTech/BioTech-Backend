using System.Security.Claims;

namespace FeedingService.Presentation.Middlewares;


/// <summary>
/// Middleware that validates requests come from the API Gateway and extracts user information from headers
/// This is the recommended approach (Option 2) where only the Gateway validates JWT tokens
/// </summary>
public class GatewayAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GatewayAuthenticationMiddleware> _logger;

    public GatewayAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<GatewayAuthenticationMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for health check endpoints
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        // Validate request comes from Gateway
        if (!ValidateGatewayRequest(context))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = "Unauthorized: Invalid gateway token or source",
                timestamp = DateTime.UtcNow
            });
            return;
        }

        // Extract user information from headers sent by Gateway
        var userClaims = ExtractUserClaims(context);
        
        if (userClaims.Any())
        {
            var identity = new ClaimsIdentity(userClaims, "Gateway");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }

    private bool ValidateGatewayRequest(HttpContext context)
    {
        // Validation 1: Check shared secret
        var gatewaySecret = context.Request.Headers["X-Gateway-Secret"].FirstOrDefault();
        var expectedSecret = _configuration["Gateway:Secret"];

        if (string.IsNullOrEmpty(gatewaySecret) || gatewaySecret != expectedSecret)
        {
            _logger.LogWarning("Request rejected: Invalid or missing gateway secret from IP {IP}", 
                context.Connection.RemoteIpAddress);
            return false;
        }

        // Validation 2: Optional IP whitelist validation
        var allowedIPs = _configuration.GetSection("Gateway:AllowedIPs").Get<string[]>();
        if (allowedIPs != null && allowedIPs.Length > 0)
        {
            var remoteIP = context.Connection.RemoteIpAddress?.ToString();
            if (remoteIP == null || !allowedIPs.Contains(remoteIP))
            {
                _logger.LogWarning("Request rejected: IP {IP} not in whitelist", remoteIP);
                return false;
            }
        }

        return true;
    }

    private List<Claim> ExtractUserClaims(HttpContext context)
    {
        var claims = new List<Claim>();

        // Extract user ID
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim("userId", userId));
        }

        // Extract email
        var email = context.Request.Headers["X-User-Email"].FirstOrDefault();
        if (!string.IsNullOrEmpty(email))
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        // Extract username
        var username = context.Request.Headers["X-User-Name"].FirstOrDefault();
        if (!string.IsNullOrEmpty(username))
        {
            claims.Add(new Claim(ClaimTypes.Name, username));
        }

        // Extract roles
        var roles = context.Request.Headers["X-User-Roles"].FirstOrDefault();
        if (!string.IsNullOrEmpty(roles))
        {
            foreach (var role in roles.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }
        }

        // Extract farm ID (business-specific claim)
        var farmId = context.Request.Headers["X-Farm-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(farmId))
        {
            claims.Add(new Claim("farmId", farmId));
        }

        return claims;
    }
}