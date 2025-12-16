using FeedingService.Domain.Entities;
using FeedingService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FeedingService.Infrastructure.Persistence.Configurations;

public class FeedingEventConfiguration : IEntityTypeConfiguration<FeedingEvent>
{
    public void Configure(EntityTypeBuilder<FeedingEvent> builder)
    {
        builder.ToTable("feeding_events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.FarmId)
            .HasColumnName("farm_id")
            .IsRequired();

        builder.Property(e => e.SupplyDate)
            .HasColumnName("supply_date")
            .HasColumnType("date")
            .HasDefaultValueSql("CURRENT_DATE")
            .IsRequired();

        builder.Property(e => e.DietId)
            .HasColumnName("diet_id");

        builder.Property(e => e.BatchId)
            .HasColumnName("batch_id");

        builder.Property(e => e.AnimalId)
            .HasColumnName("animal_id");

        builder.Property(e => e.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(e => e.TotalQuantity)
            .HasColumnName("total_quantity")
            .HasColumnType("numeric(12, 2)")
            .IsRequired();

        builder.Property(e => e.AnimalsFedCount)
            .HasColumnName("animals_fed_count")
            .IsRequired();

        builder.Property(e => e.Observations)
            .HasColumnName("observations")
            .HasColumnType("text");

        builder.Property(e => e.RegisteredBy)
            .HasColumnName("registered_by");

        // Value Object configuration
        builder.Property(e => e.UnitCostAtMoment)
            .HasColumnName("unit_cost_at_moment")
            .HasColumnType("numeric(12, 2)")
            .HasConversion(
                v => v.Amount,
                v => Money.FromDecimal(v))
            .IsRequired();

        builder.Property(e => e.CalculatedTotalCost)
            .HasColumnName("calculated_total_cost")
            .HasColumnType("numeric(12, 2)")
            .HasConversion(
                v => v != null ? v.Amount : (decimal?)null,
                v => v.HasValue ? Money.FromDecimal(v.Value) : null);

        // Audit fields
        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.LastModifiedBy)
            .HasColumnName("last_modified_by");

        // Indexes
        builder.HasIndex(e => e.FarmId)
            .HasDatabaseName("ix_feeding_events_farm_id");

        builder.HasIndex(e => e.BatchId)
            .HasDatabaseName("ix_feeding_events_batch_id");

        builder.HasIndex(e => e.AnimalId)
            .HasDatabaseName("ix_feeding_events_animal_id");
            
        builder.HasIndex(e => e.ProductId)
            .HasDatabaseName("ix_feeding_events_product_id");

        builder.HasIndex(e => e.SupplyDate)
            .HasDatabaseName("ix_feeding_events_supply_date");

        builder.HasIndex(e => new { e.FarmId, e.SupplyDate })
            .HasDatabaseName("ix_feeding_events_farm_supply_date");

        // Check constraint
        builder.ToTable(t => t.HasCheckConstraint(
            "check_feeding_destination",
            "(batch_id IS NOT NULL) <> (animal_id IS NOT NULL)")); // XOR logic in SQL
    }
}