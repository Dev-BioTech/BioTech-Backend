using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddGlobalCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                {
                    // Allow localhost for development (any port)
                    if (origin.StartsWith("http://localhost")) return true;
                    if (origin.StartsWith("https://localhost")) return true;
                    
                    // Allow Vercel deployments
                    if (origin.EndsWith(".vercel.app")) return true;
                    
                    // Allow Railway deployments
                    if (origin.EndsWith(".railway.app")) return true;

                    return false;
                })
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });

        return services;
    }
}
