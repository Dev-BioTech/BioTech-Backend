using Microsoft.Extensions.DependencyInjection;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;

namespace AuthService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
