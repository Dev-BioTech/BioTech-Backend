using FeedingService.Application.Interfaces;
using FeedingService.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using Npgsql;
using FeedingService.Infrastructure.HealthChecks;
using FeedingService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FeedingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Load .env variables
        Env.Load();

        // Database
        services.AddDbContext<FeedingDbContext>(options =>
        {
            var pgUri = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI");
            string connectionString;

            if (!string.IsNullOrEmpty(pgUri))
            {
                connectionString = pgUri;
            }
            else
            {
                var host = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_HOST") ?? Environment.GetEnvironmentVariable("DB_HOST");
                var port = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PORT") ?? Environment.GetEnvironmentVariable("DB_PORT");
                var database = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_NAME");
                var user = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
                var password = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
                var sslMode = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Disable";

                connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SslMode={sslMode};";
            }

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(FeedingDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });

            if (Environment.GetEnvironmentVariable("DB_SENSITIVE_LOGGING") == "true")
            {
                options.EnableSensitiveDataLogging();
            }
        });

        // Repositories
        services.AddScoped<IFeedingEventRepository, FeedingEventRepository>();

        // Health Checks
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database")
            .AddDbContextCheck<FeedingDbContext>("dbcontext");

        return services;
    }
}