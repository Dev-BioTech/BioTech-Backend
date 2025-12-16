using HerdService.Domain.Common;
using HerdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Persistence;

public class HerdDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Breed> Breeds { get; set; }
    public DbSet<AnimalCategory> AnimalCategories { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Paddock> Paddocks { get; set; }
    public DbSet<MovementType> MovementTypes { get; set; }
    public DbSet<AnimalMovement> AnimalMovements { get; set; }

    public HerdDbContext(DbContextOptions<HerdDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HerdDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(ct);
    }
}
