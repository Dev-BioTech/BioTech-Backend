using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Persistence;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;
    public DbSet<UserFarmRole> UserFarmRoles { get; set; } = null!;
    public DbSet<Tenant> Tenants { get; set; } = null!;
    public DbSet<Farm> Farms { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        
        // Ensure manual mappings that aren't in configurations are kept if necessary,
        // OR rely on the assumption that other configurations exist or will be created.
        // Looking at the file content, many entities are manually configured here.
        // It is safer to KEEP the existing manual configurations for other entities 
        // and ONLY REMOVE/REPLACE the Farm configuration.
        // However, "ApplyConfigurationsFromAssembly" is a cleaner pattern.
        // But to avoid breaking changes to other entities without checking if they have configurations,
        // I will keep the existing manual configurations for non-Farm entities and remove the Farm block.

        // Map to existing tables (lowercase as per SQL provided)
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Role>().ToTable("roles");
        modelBuilder.Entity<Permission>().ToTable("permissions");
        modelBuilder.Entity<RolePermission>().ToTable("role_permissions");
        modelBuilder.Entity<UserFarmRole>().ToTable("user_farm_role");

        // Configure Primary Keys and Relationships
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        modelBuilder.Entity<UserFarmRole>()
            .HasIndex(ufr => new { ufr.UserId, ufr.FarmId })
            .IsUnique();

        // Column mappings (snake_case to PascalCase)
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
        });

        modelBuilder.Entity<UserFarmRole>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
        });

        // Tenant Configuration
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("tenants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });
        
        // Farm Configuration is now handled by FarmConfiguration class via ApplyConfigurationsFromAssembly
        // or effectively by the added file if I use ApplyConfigurationsFromAssembly.
        // NOTE: Since I am not refactoring the WHOLE context to use configurations, I should manually instantiate it 
        // OR just add the `new FarmConfiguration().Configure(modelBuilder.Entity<Farm>());` line if I want to be explicit without reflection
        // BUT the best practice is consistent. 
        // Given I just added FarmConfiguration, I will trust the user won't mind if I use `ApplyConfigurationsFromAssembly` 
        // OR simply call the configuration.
        // Let's use `new FarmConfiguration().Configure(modelBuilder.Entity<Farm>());` to be safe and explicit alongside others.
        
        new Configurations.FarmConfiguration().Configure(modelBuilder.Entity<Farm>());
    }
}
