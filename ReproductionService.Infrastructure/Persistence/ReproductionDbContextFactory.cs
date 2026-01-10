using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;
using System.IO;
using ReproductionService.Infrastructure.Persistence;

namespace ReproductionService.Infrastructure.Persistence;

public class ReproductionDbContextFactory : IDesignTimeDbContextFactory<ReproductionDbContext>
{
    public ReproductionDbContext CreateDbContext(string[] args)
    {
        // Load .env variables from the Presentation project
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "ReproductionService.Presentation", ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }
        else
        {
             Env.Load();
        }

        var host = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_HOST") ?? Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PORT") ?? Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_DATABASE") ?? Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
        var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

        var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};SslMode={sslMode};TrustServerCertificate=true;";

        var optionsBuilder = new DbContextOptionsBuilder<ReproductionDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ReproductionDbContext(optionsBuilder.Options);
    }
}
