using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SalesService.Presentation.Middlewares
{
    public class GatewayAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GatewayAuthenticationMiddleware> _logger;

        public GatewayAuthenticationMiddleware(RequestDelegate next, ILogger<GatewayAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.Request.Headers["X-User-Id"].ToString();
            var userEmail = context.Request.Headers["X-User-Email"].ToString();
            var userName = context.Request.Headers["X-User-Name"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                _logger.LogInformation("[GatewayAuth] User Identity found in headers: ID={UserId}, Email={Email}", userId, userEmail);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim("userId", userId)
                };

                if (!string.IsNullOrEmpty(userEmail))
                    claims.Add(new Claim(ClaimTypes.Email, userEmail));

                if (!string.IsNullOrEmpty(userName))
                    claims.Add(new Claim(ClaimTypes.Name, userName));

                var identity = new ClaimsIdentity(claims, "GatewayAuth");
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }
}
