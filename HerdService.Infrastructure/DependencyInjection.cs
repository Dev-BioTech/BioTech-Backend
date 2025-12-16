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
        Env.TraversePath().Load();

        var connectionString = $"Host={Env.GetString("DB_HOST")};Port={Env.GetString("DB_PORT")};Database={Env.GetString("DB_DATABASE")};Username={Env.GetString("DB_USER")};Password={Env.GetString("DB_PASSWORD")};SslMode={Env.GetString("DB_SSL_MODE")}";

        services.AddDbContext<HerdDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IBreedRepository, BreedRepository>();
        services.AddScoped<IAnimalCategoryRepository, AnimalCategoryRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IPaddockRepository, PaddockRepository>();
        services.AddScoped<IMovementTypeRepository, MovementTypeRepository>();
        services.AddScoped<IAnimalMovementRepository, AnimalMovementRepository>();

        return services;
    }
}
