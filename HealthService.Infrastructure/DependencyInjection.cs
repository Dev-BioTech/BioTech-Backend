using HealthService.Application.Interfaces;
using HealthService.Infrastructure.Persistence;
using HealthService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

namespace HealthService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Load .env variables
        Env.Load();

        // Database
        services.AddDbContext<HealthServiceDbContext>(options =>
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "biotech_db"; // Default or specific name
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

            var connectionString =
                $"Host={host};Port={port};Database={database};Username={user};Password={password};SslMode={sslMode};";

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(HealthServiceDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
        });

        // Repositories
        services.AddScoped<IHealthEventRepository, HealthEventRepository>();
        
        // Messaging
        services.AddSingleton<IEventBus, HealthService.Infrastructure.Messaging.InMemoryEventBus>();

        // Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<HealthServiceDbContext>("dbcontext");

        return services;
    }
}
