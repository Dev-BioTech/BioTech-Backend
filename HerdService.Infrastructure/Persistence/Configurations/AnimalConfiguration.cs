using HerdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HerdService.Infrastructure.Persistence.Configurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(e => e.Identifier).IsRequired().HasMaxLength(50);
        builder.Property(e => e.BreedId).IsRequired();

        builder.HasOne(e => e.BreedEntity)
            .WithMany()
            .HasForeignKey(e => e.BreedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Batch)
            .WithMany()
            .HasForeignKey(e => e.BatchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Paddock)
            .WithMany()
            .HasForeignKey(e => e.PaddockId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
