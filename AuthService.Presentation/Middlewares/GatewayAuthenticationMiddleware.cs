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
        // Skip authentication for health check endpoints and Swagger
        if (context.Request.Path.StartsWithSegments("/health") || 
            context.Request.Path.StartsWithSegments("/swagger"))
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
                message = "Unauthorized: Invalid gateway token or source. Use X-Gateway-Secret in Swagger.",
                timestamp = DateTime.UtcNow
            });
            return;
        }

        // Extract user information from headers sent by Gateway (for simulate header injection)
        var userClaims = ExtractUserClaims(context);
        
        // DEV MODE: If no headers and in Development, inject default test user
        if (!userClaims.Any() && _env.IsDevelopment())
        {
            _logger.LogInformation("Dev Mode: Injecting default test user claims for manual testing");
            userClaims = GetDefaultDevClaims();
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
