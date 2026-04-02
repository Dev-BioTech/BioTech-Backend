using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace HealthService.Presentation.Middlewares;

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
            _logger.LogWarning("Unauthorized: Gateway validation failed for request to {Path} from {IP}", 
                context.Request.Path, context.Connection.RemoteIpAddress);

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
        
        // Auto-Inject Mock User for Local Development (if no headers present)
        if (!userClaims.Any())
        {
             var env = context.RequestServices.GetService<IWebHostEnvironment>();
             if (env != null && env.IsDevelopment())
             {
                 var remoteIp = context.Connection.RemoteIpAddress;
                 if (remoteIp != null && IPAddress.IsLoopback(remoteIp))
                 {
                     _logger.LogWarning("Identity Injection: Using Mock Admin User for Local Development.");
                     userClaims.Add(new Claim(ClaimTypes.NameIdentifier, "debug-user-id"));
                     userClaims.Add(new Claim("userId", "debug-user-id"));
                     userClaims.Add(new Claim(ClaimTypes.Name, "Debug Admin"));
                     userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                     userClaims.Add(new Claim(ClaimTypes.Email, "admin@debug.local"));
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
        var env = context.RequestServices.GetService<IWebHostEnvironment>();
        if (env != null && env.IsDevelopment())
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

        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(userId))
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            claims.Add(new Claim("userId", userId));
        }

        var email = context.Request.Headers["X-User-Email"].FirstOrDefault();
        if (!string.IsNullOrEmpty(email))
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        var username = context.Request.Headers["X-User-Name"].FirstOrDefault();
        if (!string.IsNullOrEmpty(username))
        {
            claims.Add(new Claim(ClaimTypes.Name, username));
        }

        var roles = context.Request.Headers["X-User-Roles"].FirstOrDefault();
        if (!string.IsNullOrEmpty(roles))
        {
            foreach (var role in roles.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }
        }

        var farmId = context.Request.Headers["X-Farm-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(farmId))
        {
            claims.Add(new Claim("farmId", farmId));
        }

        return claims;
    }
}
