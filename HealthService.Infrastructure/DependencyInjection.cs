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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Fallback for direct instantiation scenarios where configuration might be missing
            if (string.IsNullOrEmpty(connectionString)) 
            {
                 // Try to build it robustly if missing from config (e.g. design time)
                 var pgUri = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI");
                 if (!string.IsNullOrEmpty(pgUri) && pgUri.StartsWith("postgresql://"))
                 {
                     try 
                     {
                        var uri = new Uri(pgUri);
                        var userInfo = uri.UserInfo.Split(':');
                        connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true;";
                     }
                     catch { /* use defaults */ }
                 }
            }

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
