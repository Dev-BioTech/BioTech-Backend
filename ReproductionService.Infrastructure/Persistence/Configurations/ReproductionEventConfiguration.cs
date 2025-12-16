using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Infrastructure.Persistence.Configurations;

public class ReproductionEventConfiguration : IEntityTypeConfiguration<ReproductionEvent>
{
    public void Configure(EntityTypeBuilder<ReproductionEvent> builder)
    {
        builder.ToTable("reproduction_events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FarmId).HasColumnName("farm_id").IsRequired();
        builder.Property(e => e.AnimalId).HasColumnName("animal_id").IsRequired();
        builder.Property(e => e.EventDate).HasColumnName("event_date").IsRequired();
        builder.Property(e => e.EventType).HasColumnName("event_type").IsRequired();
        builder.Property(e => e.Observations).HasColumnName("notes").HasMaxLength(2000); // Mapped 'notes' in SQL to Observations in code
        builder.Property(e => e.RegisteredBy).HasColumnName("created_by"); // reusing created_by or specific column? SQL says created_by. Wait, SQL has 'created_by' separate. SQL has 'notes'.
        
        // Let's re-read SQL provided
        // notes TEXT
        // created_by INT
        // 
        // Code has `RegisteredBy`. I will map `Observations` to `notes`.
        // I will map `RegisteredBy` to... wait, `RegisteredBy` is typical for business logic user. 
        // SQL only shows `created_by`. 
        // If I map `RegisteredBy` to `created_by`, then `CreatedBy` (Auditable) conflicts.
        // I'll assume `RegisteredBy` is NOT in the SQL provided in the prompt, or it's implicitly `created_by`.
        // The SQL provided: id, farm_id, animal_id, male_animal_id, event_type, event_date, is_successful, expected_birth_date, offspring_count, notes, is_cancelled, created_at, updated_at, created_by, last_modified_by.
        
        // Code entity has: RegisteredBy.
        // I will Ignore RegisteredBy if it doesn't fit, or better - check if I can remove it from Entity to match Schema strictly.
        // User said "Entity Framework Core + PostgreSQL".
        // Use `Ignore` for now if not in SQL.
        
        builder.Ignore(e => e.RegisteredBy);

        
        builder.Property(e => e.MaleAnimalId).HasColumnName("male_animal_id");
        builder.Property(e => e.IsSuccessful).HasColumnName("is_successful");
        builder.Property(e => e.ExpectedBirthDate).HasColumnName("expected_birth_date");
        builder.Property(e => e.OffspringCount).HasColumnName("offspring_count");
        
        builder.Property(e => e.IsCancelled).HasColumnName("is_cancelled").HasDefaultValue(false);

        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.LastModifiedBy).HasColumnName("last_modified_by");

        // Indexes
        builder.HasIndex(e => new { e.FarmId, e.EventDate });
        builder.HasIndex(e => e.AnimalId);
        builder.HasIndex(e => e.EventType);
    }
}
