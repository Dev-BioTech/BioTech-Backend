using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReproductionService.Application.Interfaces;
using ReproductionService.Infrastructure.Persistence;
using ReproductionService.Infrastructure.Repositories;

namespace ReproductionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Use the connection string already configured in Program.cs
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Fallback to localhost if nothing is configured
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Host=localhost;Database=reproduction_db;Username=postgres;Password=postgres;";
        }

        services.AddDbContext<ReproductionDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IReproductionEventRepository, ReproductionEventRepository>();

        return services;
    }
}
