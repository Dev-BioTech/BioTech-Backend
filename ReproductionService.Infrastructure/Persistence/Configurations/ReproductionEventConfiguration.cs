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
        builder.Property(e => e.EventDate).HasColumnName("reproduction_date").IsRequired();
        builder.Property(e => e.EventType).HasColumnName("reproduction_type").IsRequired();
        builder.Property(e => e.Observations).HasColumnName("observations").HasMaxLength(2000);
        builder.Property(e => e.RegisteredBy).HasColumnName("registered_by");
        
        builder.Property(e => e.MaleAnimalId).HasColumnName("male_animal_id");
        builder.Property(e => e.SemenBatchId).HasColumnName("semen_batch_id");
        builder.Property(e => e.PregnancyResult).HasColumnName("pregnancy_result");
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
