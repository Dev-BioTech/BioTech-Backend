using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommercialService.Domain.Interfaces;
using CommercialService.Infrastructure.Persistence;

namespace CommercialService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CommercialDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICommercialRepository, CommercialRepository>();
        services.AddScoped<IThirdPartyRepository, ThirdPartyRepository>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration["RABBITMQ_HOST"];
                var user = configuration["RABBITMQ_USER"];
                var pass = configuration["RABBITMQ_PASS"];

                if (!string.IsNullOrEmpty(host))
                {
                    cfg.Host(host, "/", h =>
                    {
                        h.Username(user ?? "guest");
                        h.Password(pass ?? "guest");
                    });
                }
                else
                {
                    // Fallback for local development if no RabbitMQ config provided
                    // Logic: If we want to test locally without Rabbit, we can use InMemory
                    // But typically MassTransit.RabbitMQ requires Rabbit.
                    // Let's use InMemory if no host, BUT 'UsingRabbitMq' creates a Rabbit transport.
                    // Better approach: Check config BEFORE configuring transport.
                    // However, replacing the whole block logic is cleaner. 
                    // Let's assume for Railway readiness we default to Rabbit, but if host is localhost we use it.
                    // Creating an InMemory fallback inside UsingRabbitMq is not possible.
                    // We need to decide transport at root.
                    // Let's keep it simple: If config exists, use it. Else localhost.
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
