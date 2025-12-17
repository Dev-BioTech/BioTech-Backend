using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Interfaces;
using Shared.Infrastructure.Services;

namespace AIService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        services.AddHttpClient<IMessenger, HttpMessenger>();
        
        return services;
    }
}
