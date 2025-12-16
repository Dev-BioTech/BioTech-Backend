using Microsoft.EntityFrameworkCore;
using ReproductionService.Domain.Common;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Infrastructure.Persistence;

public class ReproductionDbContext : DbContext
{
    public DbSet<ReproductionEvent> ReproductionEvents { get; set; }

    public ReproductionDbContext(DbContextOptions<ReproductionDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations if any
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReproductionDbContext).Assembly);
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
