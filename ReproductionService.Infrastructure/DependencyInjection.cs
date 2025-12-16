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
        // Load .env variables
        Env.TraversePath().Load();

        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("DB_DATABASE");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable";

        var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SslMode={sslMode};";

        services.AddDbContext<ReproductionDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IReproductionEventRepository, ReproductionEventRepository>();

        return services;
    }
}
