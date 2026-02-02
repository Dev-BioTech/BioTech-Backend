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

            options.UseNpgsql(connectionString);
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
