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
                var database = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_DATABASE");
                var user = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
                var password = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
                var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

                connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SslMode={sslMode};";
            }

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
