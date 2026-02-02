using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.Repositories;
using DotNetEnv;

namespace InventoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Load .env variables
        Env.Load();

        services.AddDbContext<InventoryDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Fallback: If null, try to construct it (safety net)
            if (string.IsNullOrEmpty(connectionString)) 
            {
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

        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
