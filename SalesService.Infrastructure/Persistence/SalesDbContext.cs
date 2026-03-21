using Microsoft.EntityFrameworkCore;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Persistence;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    public DbSet<Sale> Sales { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BuyerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });
    }
}
