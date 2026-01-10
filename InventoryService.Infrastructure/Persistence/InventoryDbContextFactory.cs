using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System;
using System.IO;
using InventoryService.Infrastructure.Persistence;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoryDbContext>
{
    public InventoryDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"[DEBUG] CurrentDirectory: {Directory.GetCurrentDirectory()}");
        
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "InventoryService.Presentation", ".env");
        Console.WriteLine($"[DEBUG] Expected .env path: {envPath}");

        if (File.Exists(envPath))
        {
            Console.WriteLine("[DEBUG] .env file FOUND. Loading...");
            Env.Load(envPath);
        }
        else
        {
            Console.WriteLine("[DEBUG] .env file NOT FOUND at expected path.");
            Env.Load(); // Try default
        }

        var host = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_HOST") ?? Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PORT") ?? Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_DB") ?? Environment.GetEnvironmentVariable("DB_DATABASE");
        var user = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_USER") ?? Environment.GetEnvironmentVariable("DB_USER");
        // var password = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
        var sslMode = Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Require";

        // Hardcode password check or mask
        var pass = Environment.GetEnvironmentVariable("POSTGRESQL_ADDON_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD");
        var passLog = string.IsNullOrEmpty(pass) ? "NULL/EMPTY" : "***";

        var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={pass};SslMode={sslMode};TrustServerCertificate=true;";
        Console.WriteLine($"[DEBUG] Connection String: Host={host};Port={port};Database={database};Username={user};Password={passLog};SslMode={sslMode};");

        var optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new InventoryDbContext(optionsBuilder.Options);
    }
}
