using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<InventoryMovement> InventoryMovements { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!; // New

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to Snake Case tables
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<InventoryMovement>().ToTable("inventory_movements");

        // Property Mappings (Snake Case Columns)
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
            entity.Property(e => e.Category).HasColumnName("category").HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.UnitOfMeasure).HasColumnName("unit_of_measure").HasMaxLength(20);
            entity.Property(e => e.CurrentQuantity).HasColumnName("current_quantity").HasColumnType("numeric(12,2)");
            entity.Property(e => e.AverageCost).HasColumnName("average_cost").HasColumnType("numeric(12,2)");
            entity.Property(e => e.MinimumStock).HasColumnName("minimum_stock").HasColumnType("numeric(12,2)");
            entity.Property(e => e.Active).HasColumnName("active");

            entity.HasIndex(e => new { e.FarmId, e.Name }).IsUnique().HasDatabaseName("uk_product_name_farm");
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.Property(e => e.MovementDate).HasColumnName("movement_date").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.MovementType).HasColumnName("movement_type").HasConversion<string>().HasMaxLength(10);
            entity.Property(e => e.Concept).HasColumnName("concept").HasConversion<string>().HasMaxLength(30);

            entity.Property(e => e.Quantity).HasColumnName("quantity").HasColumnType("numeric(12,2)");

            entity.Property(e => e.TransactionUnitCost).HasColumnName("transaction_unit_cost").HasColumnType("numeric(12,2)");
            entity.Property(e => e.TransactionTotalCost).HasColumnName("transaction_total_cost").HasColumnType("numeric(12,2)");

            entity.Property(e => e.SubsequentQuantityBalance).HasColumnName("subsequent_quantity_balance").HasColumnType("numeric(12,2)");
            entity.Property(e => e.SubsequentAverageCost).HasColumnName("subsequent_average_cost").HasColumnType("numeric(12,2)");

            entity.Property(e => e.ThirdPartyId).HasColumnName("third_party_id");
            entity.Property(e => e.ReferenceDocument).HasColumnName("reference_document").HasMaxLength(50);
            entity.Property(e => e.Observations).HasColumnName("observations").HasMaxLength(255);
            entity.Property(e => e.RegisteredBy).HasColumnName("registered_by");
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("animals");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FarmId).HasColumnName("farm_id");

            entity.Property(e => e.VisualCode).HasColumnName("visual_code").HasMaxLength(20);
            entity.Property(e => e.ElectronicCode).HasColumnName("electronic_code").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);

            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Sex).HasColumnName("sex").HasMaxLength(1);
            entity.Property(e => e.BreedId).HasColumnName("breed_id");

            entity.Property(e => e.MotherId).HasColumnName("mother_id");
            entity.Property(e => e.FatherId).HasColumnName("father_id");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CurrentStatus).HasColumnName("current_status").HasMaxLength(20);

            entity.Property(e => e.InitialCost).HasColumnName("initial_cost").HasColumnType("numeric(12,2)").HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasIndex(e => new { e.FarmId, e.VisualCode }).IsUnique().HasDatabaseName("uk_animal_code_farm");
        });
    }
}
