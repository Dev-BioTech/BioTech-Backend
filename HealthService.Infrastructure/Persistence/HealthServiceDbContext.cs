using HealthService.Domain.Common;
using HealthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthService.Infrastructure.Persistence;

public class HealthServiceDbContext : DbContext
{
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<HealthEvent> HealthEvents { get; set; }
    public DbSet<HealthEventDetail> HealthEventDetails { get; set; }
    public DbSet<WithdrawalPeriod> WithdrawalPeriods { get; set; }

    public HealthServiceDbContext(DbContextOptions<HealthServiceDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Map to specific table names as per SQL provided
        modelBuilder.Entity<Disease>().ToTable("diseases");
        modelBuilder.Entity<HealthEvent>().ToTable("health_events");
        modelBuilder.Entity<HealthEventDetail>().ToTable("health_event_details");
        modelBuilder.Entity<WithdrawalPeriod>().ToTable("withdrawal_periods");

        modelBuilder.Entity<HealthEventDetail>()
            .OwnsOne(e => e.UnitCostAtMoment, m =>
            {
                m.Property(p => p.Amount).HasColumnName("unit_cost_at_moment");
            });

        modelBuilder.Entity<HealthEventDetail>()
            .OwnsOne(e => e.CalculatedTotalCost, m =>
            {
                m.Property(p => p.Amount).HasColumnName("calculated_total_cost");
            });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HealthServiceDbContext).Assembly);
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
