using System.Security.Claims;
using System.Net;

namespace AuthService.Presentation.Middlewares;

/// <summary>
/// Middleware that validates requests come from the API Gateway and extracts user information from headers
/// This allows direct service testing with X-Gateway-Secret in development
/// </summary>
public class GatewayAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GatewayAuthenticationMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GatewayAuthenticationMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<GatewayAuthenticationMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for health check endpoints
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        // Diagnostic log: See what headers we are actually receiving from the Gateway
        var headers = string.Join(" | ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"));
        _logger.LogInformation("[GatewayAuth] Incoming Headers: {Headers}", headers);

        // Check if allow gateway secret is present
        string? gatewaySecret = context.Request.Headers["X-Gateway-Secret"].FirstOrDefault();

        if (string.IsNullOrEmpty(gatewaySecret))
        {
            // Fallback to standard authentication if no gateway secret (internal/direct calls)
            await _next(context);
            return;
        }

        // Validate request comes from Gateway
        if (!ValidateGatewayRequest(context))
        {
            _logger.LogWarning("Unauthorized: Gateway validation failed for request to {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "Unauthorized: Invalid gateway token or source" });
            return;
        }

        // Extract user information from headers sent by Gateway
        var userClaims = ExtractUserClaims(context);
        
        // Final Backstop: If headers are missing but Authorization header is present, try manual extraction
        if (!userClaims.Any(c => c.Type == ClaimTypes.NameIdentifier))
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    if (handler.CanReadToken(token))
                    {
                        var jwtToken = handler.ReadJwtToken(token);
                        var userId = jwtToken.Subject ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            _logger.LogWarning("[GatewayAuth] IDENTITY RECOVERY: Extracted userId {UserId} directly from JWT because X-User-Id header was missing.", userId);
                            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
                            userClaims.Add(new Claim("userId", userId));
                            
                            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                            if (!string.IsNullOrEmpty(email)) userClaims.Add(new Claim(ClaimTypes.Email, email));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[GatewayAuth] Failed to manually recover identity from JWT.");
                }
            }
        }

        if (userClaims.Any())
        {
            var identity = new ClaimsIdentity(userClaims, "Gateway");
            context.User = new ClaimsPrincipal(identity);
        }

        await _next(context);
    }

    private bool ValidateGatewayRequest(HttpContext context)
    {
        // 0. Bypass for Localhost in Development
        if (_env.IsDevelopment())
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            if (remoteIp != null && IPAddress.IsLoopback(remoteIp))
            {
                _logger.LogWarning("Security Bypass: Allowing Localhost request in Development mode.");
                return true;
            }
        }

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
        
        // Also support comma-separated string if the array is null or empty
        if (allowedIPs == null || allowedIPs.Length == 0)
        {
            var allowedIPsString = _configuration["Gateway:AllowedIPs"];
            if (!string.IsNullOrEmpty(allowedIPsString))
            {
                allowedIPs = allowedIPsString.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ip => ip.Trim()).ToArray();
            }
        }

        if (allowedIPs != null && allowedIPs.Length > 0)
        {
            // Try to get IP from X-Forwarded-For if it exists (for proxies/load balancers)
            string? remoteIP = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(remoteIP))
            {
                // Take the first IP if multiple are present
                remoteIP = remoteIP.Split(',')[0].Trim();
            }
            else
            {
                remoteIP = context.Connection.RemoteIpAddress?.ToString();
            }

            if (remoteIP == null || !allowedIPs.Any(ip => ip.Trim() == remoteIP))
            {
                _logger.LogWarning("Request rejected: IP {IP} not in whitelist. Allowed IPs: {AllowedIPs}", 
                    remoteIP ?? "Unknown", string.Join(", ", allowedIPs));
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

        // Extract farm ID
        var farmId = context.Request.Headers["X-Farm-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(farmId))
        {
            claims.Add(new Claim("farmId", farmId));
        }

        return claims;
    }

    private List<Claim> GetDefaultDevClaims()
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim("userId", "1"),
            new Claim(ClaimTypes.Email, "dev@biotech.com"),
            new Claim(ClaimTypes.Name, "Dev Admin"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Owner"),
            new Claim("farmId", "1"),
            new Claim("farm_role", "1:Owner")
        };
    }
}
