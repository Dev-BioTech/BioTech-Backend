using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.Infrastructure.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// Applies pending migrations for the specified DbContext if the environment is Production.
    /// </summary>
    /// <typeparam name="T">The type of DbContext.</typeparam>
    /// <param name="app">The IApplicationBuilder instance.</param>
    public static void ApplyMigrations<T>(this IApplicationBuilder app) where T : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        
        // Only run migrations in Production as requested
        if (!environment.IsProduction())
        {
            return;
        }

        using T context = scope.ServiceProvider.GetRequiredService<T>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<T>>();

        try
        {
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();

            if (pendingMigrations.Count > 0)
            {
                logger.LogInformation("Applying {Count} pending migrations for {DbContext}...", pendingMigrations.Count, typeof(T).Name);
                context.Database.Migrate();
                logger.LogInformation("Migrations applied successfully for {DbContext}.", typeof(T).Name);
            }
            else
            {
                logger.LogInformation("Database is already up to date for {DbContext}.", typeof(T).Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations for {DbContext}.", typeof(T).Name);
            // In production, we might want to fail fast if migrations fail
            throw;
        }
    }
}
