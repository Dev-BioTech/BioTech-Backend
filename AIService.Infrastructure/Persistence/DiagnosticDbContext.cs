using AIService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIService.Infrastructure.Persistence;

public class DiagnosticDbContext : DbContext
{
    public DiagnosticDbContext(DbContextOptions<DiagnosticDbContext> options) : base(options)
    {
    }

    public DbSet<DiagnosticSession> DiagnosticSessions { get; set; }
    public DbSet<ErrorPattern> ErrorPatterns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DiagnosticSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId).IsUnique();
            entity.HasIndex(e => e.ServiceName);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.StartedAt);
        });

        modelBuilder.Entity<ErrorPattern>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PatternName).IsUnique();
        });
    }
}
