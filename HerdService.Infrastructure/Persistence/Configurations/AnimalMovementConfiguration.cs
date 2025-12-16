using HerdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HerdService.Infrastructure.Persistence.Configurations;

public class AnimalMovementConfiguration : IEntityTypeConfiguration<AnimalMovement>
{
    public void Configure(EntityTypeBuilder<AnimalMovement> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.MovementDate).IsRequired();
        builder.Property(e => e.Observation).HasMaxLength(500);

        builder.HasOne(e => e.Animal)
            .WithMany()
            .HasForeignKey(e => e.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.MovementType)
            .WithMany()
            .HasForeignKey(e => e.MovementTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
