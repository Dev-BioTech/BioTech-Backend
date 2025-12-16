using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Infrastructure.Persistence.Configurations;

public class ReproductionEventConfiguration : IEntityTypeConfiguration<ReproductionEvent>
{
    public void Configure(EntityTypeBuilder<ReproductionEvent> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.Cost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Cost")
                .HasColumnType("decimal(18,2)");
        });
    }
}
