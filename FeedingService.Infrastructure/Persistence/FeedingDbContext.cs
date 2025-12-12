using FeedingService.Domain.Common;
using FeedingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeedingService.Infrastructure.Persistence;

public class FeedingDbContext : DbContext
{
    public DbSet<FeedingEvent> FeedingEvents { get; set; }

    public FeedingDbContext(DbContextOptions<FeedingDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FeedingDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(ct);
    }
}