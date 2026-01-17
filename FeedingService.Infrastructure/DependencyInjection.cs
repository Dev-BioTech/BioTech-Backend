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
        // Database
        services.AddDbContext<FeedingDbContext>(options =>
        {
            // Use the connection string already configured in Program.cs
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Fallback to localhost if nothing is configured
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Host=localhost;Database=feeding_db;Username=postgres;Password=postgres;";
            }

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(FeedingDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });

            if (configuration.GetValue<bool>("Database:EnableSensitiveDataLogging"))
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