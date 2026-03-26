using HerdService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HerdService.Infrastructure.Persistence.Configurations;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> builder)
    {
        builder.ToTable("animals");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.VisualCode)
            .HasColumnName("visual_code")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.ElectronicCode)
            .HasColumnName("electronic_code")
            .HasMaxLength(50);

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(100);

        builder.Property(e => e.Color)
            .HasColumnName("color")
            .HasMaxLength(30);

        builder.Property(e => e.FarmId).HasColumnName("farm_id").IsRequired();
        builder.Property(e => e.BreedId).HasColumnName("breed_id");
        builder.Property(e => e.CategoryId).HasColumnName("category_id");
        builder.Property(e => e.BatchId).HasColumnName("batch_id");
        builder.Property(e => e.PaddockId).HasColumnName("paddock_id");

        builder.Property(e => e.BirthDate).HasColumnName("birth_date").HasColumnType("date").IsRequired();
        builder.Property(e => e.Sex).HasColumnName("sex").HasMaxLength(1).IsFixedLength().IsRequired();

        builder.Property(e => e.MotherId).HasColumnName("mother_id");
        builder.Property(e => e.FatherId).HasColumnName("father_id");
        builder.Property(e => e.ExternalMother).HasColumnName("external_mother").HasMaxLength(50);
        builder.Property(e => e.ExternalFather).HasColumnName("external_father").HasMaxLength(50);
        builder.Property(e => e.Weight).HasColumnName("weight").HasColumnType("numeric(10,2)");
        builder.Property(e => e.Height).HasColumnName("height").HasColumnType("numeric(10,2)");

        builder.Property(e => e.CurrentStatus)
            .HasColumnName("current_status")
            .HasMaxLength(20)
            .HasDefaultValue("ACTIVE")
            .IsRequired();

        builder.Property(e => e.Purpose)
            .HasColumnName("purpose")
            .HasMaxLength(20)
            .HasDefaultValue("MEAT");

        builder.Property(e => e.Origin)
            .HasColumnName("origin")
            .HasMaxLength(20);

        builder.Property(e => e.EntryDate).HasColumnName("entry_date").HasColumnType("date");
        builder.Property(e => e.InitialCost).HasColumnName("initial_cost").HasColumnType("numeric(12,2)").HasDefaultValue(0m);

        // Audit
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Indexes
        builder.HasIndex(e => new { e.FarmId, e.VisualCode }).IsUnique().HasDatabaseName("uk_animal_code_farm");
        // Other assumed indexes or remove if not in schema request (uk_animal_code_farm matched constraint)

        // Relationships
        builder.HasOne(e => e.Breed)
            .WithMany()
            .HasForeignKey(e => e.BreedId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Batch)
            .WithMany()
            .HasForeignKey(e => e.BatchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Paddock)
            .WithMany()
            .HasForeignKey(e => e.PaddockId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Mother)
            .WithMany()
            .HasForeignKey(e => e.MotherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Father)
            .WithMany()
            .HasForeignKey(e => e.FatherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Movements)
            .WithOne(m => m.Animal)
            .HasForeignKey(m => m.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
