using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace FeedingService.Presentation.Authorization;

/// <summary>
/// Options class required by the AuthenticationHandler base class.
/// No configuration is needed since trust is based purely on the
/// identity injected by GatewayAuthenticationMiddleware.
/// </summary>
public class GatewayAuthHandlerOptions : AuthenticationSchemeOptions { }

/// <summary>
/// Custom authentication handler that validates requests forwarded by the API Gateway.
///
/// The design is intentionally simple: the GatewayAuthenticationMiddleware runs first
/// and sets context.User with a ClaimsIdentity of type "Gateway". This handler then
/// confirms that identity for ASP.NET's [Authorize] attribute without re-validating
/// the original JWT (which is the Gateway's responsibility).
///
/// This avoids the double-authentication conflict where UseAuthentication() was
/// previously overriding context.User and producing 401s on every request.
/// </summary>
public class GatewayAuthenticationHandler : AuthenticationHandler<GatewayAuthHandlerOptions>
{
    public GatewayAuthenticationHandler(
        IOptionsMonitor<GatewayAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // The GatewayAuthenticationMiddleware has already populated context.User
        // with a "Gateway"-typed ClaimsIdentity. We simply confirm it here.
        var gatewayIdentity = Context.User?.Identities
            .FirstOrDefault(i => i.AuthenticationType == "Gateway");

        if (gatewayIdentity != null && gatewayIdentity.IsAuthenticated)
        {
            var ticket = new AuthenticationTicket(Context.User!, Scheme.Name);
            Logger.LogDebug("Gateway authentication succeeded for user {UserId}",
                gatewayIdentity.Name ?? "unknown");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        Logger.LogDebug("Gateway authentication: no valid Gateway identity found on context.User.");
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}
