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
        Env.Load();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Fallback safety net
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
        
        services.AddDbContext<ReproductionDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IReproductionEventRepository, ReproductionEventRepository>();
        services.AddScoped<IPregnancyRepository, PregnancyRepository>();
        services.AddScoped<IBirthRepository, BirthRepository>();

        return services;
    }
}
