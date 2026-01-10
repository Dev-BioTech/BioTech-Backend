using DotNetEnv;
using HerdService.Application.Interfaces;
using HerdService.Infrastructure.Persistence;
using HerdService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HerdService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<HerdDbContext>(options =>
        {
            var pgUri = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_URI");
            if (!string.IsNullOrEmpty(pgUri))
            {
                options.UseNpgsql(pgUri);
            }
            else
            {
                var host = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_HOST") ?? Environment.GetEnvironmentVariable("DB_HOST");
                if (!string.IsNullOrEmpty(host))
                {
                    var port = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PORT") ?? Environment.GetEnvironmentVariable("DB_PORT");
                    var database = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_DATABASE") ?? "herd_db";
                    var user = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
                    var password = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
                    var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

                    var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};Ssl Mode={sslMode};";
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                }
            }
        });

        // Repositories
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<IAnimalCategoryRepository, AnimalCategoryRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IPaddockRepository, PaddockRepository>();
        services.AddScoped<IMovementTypeRepository, MovementTypeRepository>();
        services.AddScoped<IAnimalMovementRepository, AnimalMovementRepository>();
        // Add other repositories when implemented
        
        return services;
    }
}
