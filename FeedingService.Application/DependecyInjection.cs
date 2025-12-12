using Microsoft.Extensions.DependencyInjection;

namespace FeedingService.Application;

public static IServiceCollection AddApplication(this IServiceCollection services)
{
    // Register MediatR
    services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

    // Register Validators
    services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

    return services;
}