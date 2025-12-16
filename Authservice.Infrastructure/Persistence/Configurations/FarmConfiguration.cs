using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations;

public class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable("farms");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Owner)
            .HasColumnName("owner")
            .HasMaxLength(100);

        builder.Property(e => e.Address)
            .HasColumnName("address")
            .HasMaxLength(200);

        builder.Property(e => e.GeographicLocation)
            .HasColumnName("geographic_location")
            .HasMaxLength(100);

        builder.Property(e => e.Active)
            .HasColumnName("active")
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        // Ignore fields not in DB to prevent EF errors if IAuditableEntity is kept or properties exist
        builder.Ignore(e => e.UpdatedAt);
        builder.Ignore(e => e.CreatedBy);
        builder.Ignore(e => e.LastModifiedBy);

        // No TenantUserId in farms table anymore, managed via user_farm_role
        // builder.HasIndex(e => e.TenantUserId)... REMOVE

    }
}
