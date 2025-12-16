using HealthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HealthService.Infrastructure.Persistence.Configurations;

public class HealthEventConfiguration : IEntityTypeConfiguration<HealthEvent>
{
    public void Configure(EntityTypeBuilder<HealthEvent> builder)
    {
        builder.ToTable("health_events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.FarmId)
            .HasColumnName("farm_id")
            .IsRequired();

        builder.Property(e => e.AnimalId)
            .HasColumnName("animal_id");

        builder.Property(e => e.BatchId)
            .HasColumnName("batch_id");

        builder.Property(e => e.EventType)
            .HasColumnName("event_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.EventDate)
            .HasColumnName("event_date")
            .IsRequired();

        builder.Property(e => e.Disease)
            .HasColumnName("disease")
            .HasMaxLength(200);

        builder.Property(e => e.Treatment)
            .HasColumnName("treatment");

        builder.Property(e => e.Medication)
            .HasColumnName("medication")
            .HasMaxLength(200);

        builder.Property(e => e.Dosage)
            .HasColumnName("dosage")
            .HasPrecision(10, 2);

        builder.Property(e => e.DosageUnit)
            .HasColumnName("dosage_unit")
            .HasMaxLength(20);

        builder.Property(e => e.VeterinarianName)
            .HasColumnName("veterinarian_name")
            .HasMaxLength(200);

        builder.Property(e => e.Cost)
            .HasColumnName("cost")
            .HasPrecision(12, 2);

        builder.Property(e => e.Notes)
            .HasColumnName("notes");

        builder.Property(e => e.NextFollowUpDate)
            .HasColumnName("next_follow_up_date");

        builder.Property(e => e.RequiresFollowUp)
            .HasColumnName("requires_follow_up")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.FollowUpNotes)
            .HasColumnName("follow_up_notes");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.LastModifiedBy)
            .HasColumnName("last_modified_by");

        // Indexes
        builder.HasIndex(e => e.FarmId);
        builder.HasIndex(e => e.AnimalId);
        builder.HasIndex(e => e.BatchId);
        builder.HasIndex(e => e.EventType);
        builder.HasIndex(e => e.EventDate);
        builder.HasIndex(e => new { e.FarmId, e.EventDate });
        
        // Ignore navigation properties to avoid creating bad tables if types don't exist in this context fully
        builder.Ignore(e => e.Animal);
        builder.Ignore(e => e.Batch);
    }
}
