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
            
            // Aggressive Extraction: If standard user is not authenticated or lacks ID, try manual JWT parsing
            string? userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? user?.FindFirst("sub")?.Value 
                          ?? user?.FindFirst("userId")?.Value;
            
            string? email = user?.FindFirst(ClaimTypes.Email)?.Value 
                         ?? user?.FindFirst("email")?.Value;
                         
            string? username = user?.FindFirst(ClaimTypes.Name)?.Value 
                            ?? user?.FindFirst("name")?.Value 
                            ?? user?.FindFirst("username")?.Value
                            ?? user?.FindFirst("fullName")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var token = authHeader.Substring("Bearer ".Length).Trim();
                        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                        if (handler.CanReadToken(token))
                        {
                            var jwtToken = handler.ReadJwtToken(token);
                            userId = jwtToken.Subject ?? jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                            email ??= jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                            username ??= jwtToken.Claims.FirstOrDefault(c => c.Type == "fullName" || c.Type == "name" || c.Type == "unique_name")?.Value;
                        }
                    }
                    catch { /* Ignore parsing errors, fallback to empty */ }
                }
            }

            if (!string.IsNullOrEmpty(userId))
            {
                request.Headers.Remove("X-User-Id");
                request.Headers.Add("X-User-Id", userId);
                
                if (!string.IsNullOrEmpty(email))
                {
                    request.Headers.Remove("X-User-Email");
                    request.Headers.Add("X-User-Email", email);
                }
                    
                if (!string.IsNullOrEmpty(username))
                {
                    request.Headers.Remove("X-User-Name");
                    request.Headers.Add("X-User-Name", username);
                }

                // Roles Handling
                var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();
                if (!roles.Any())
                {
                    roles = user?.FindAll("role").Select(c => c.Value).ToList() ?? new List<string>();
                }
                
                var farmRoles = user?.FindAll("farm_role").Select(c => c.Value).ToList() ?? new List<string>();
                if (!farmRoles.Any())
                {
                    // Fallback to manual JWT if needed
                    var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(authHeader))
                    {
                         try {
                            var token = authHeader.Split(" ").Last();
                            var jwt = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().ReadJwtToken(token);
                            farmRoles = jwt.Claims.Where(c => c.Type == "farm_role").Select(c => c.Value).ToList();
                         } catch {}
                    }
                }

                if (farmRoles.Any()) roles.AddRange(farmRoles);
                if (roles.Any()) request.Headers.TryAddWithoutValidation("X-User-Roles", string.Join(",", roles.Distinct()));

                // Farm ID extraction
                var farmId = user?.FindFirst("farmId")?.Value ?? user?.FindFirst("FarmId")?.Value;
                if (string.IsNullOrEmpty(farmId) && farmRoles.Any())
                {
                    var firstFarmRole = farmRoles.First();
                    if (firstFarmRole.Contains(':')) farmId = firstFarmRole.Split(':')[0];
                }
                
                if (!string.IsNullOrEmpty(farmId)) request.Headers.TryAddWithoutValidation("X-Farm-Id", farmId);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
