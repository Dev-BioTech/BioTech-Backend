using Microsoft.EntityFrameworkCore;
using CommercialService.Domain.Entities;

namespace CommercialService.Infrastructure.Persistence;

public class CommercialDbContext : DbContext
{
    public CommercialDbContext(DbContextOptions<CommercialDbContext> options) : base(options)
    {
    }

    public DbSet<CommercialTransaction> Transactions { get; set; } = null!;
    public DbSet<TransactionAnimalDetail> AnimalDetails { get; set; } = null!;
    public DbSet<TransactionProductDetail> ProductDetails { get; set; } = null!;
    public DbSet<ThirdParty> ThirdParties { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map tables
        modelBuilder.Entity<CommercialTransaction>().ToTable("commercial_transactions");
        modelBuilder.Entity<TransactionAnimalDetail>().ToTable("transaction_animal_details");
        modelBuilder.Entity<TransactionProductDetail>().ToTable("transaction_product_details");

        // Primary Keys & Properties
        modelBuilder.Entity<CommercialTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.ThirdPartyId).HasColumnName("third_party_id");
            entity.Property(e => e.TransactionType).HasColumnName("transaction_type").HasConversion<string>();
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.InvoiceNumber).HasColumnName("invoice_number");
            entity.Property(e => e.Subtotal).HasColumnName("subtotal");
            entity.Property(e => e.Taxes).HasColumnName("taxes");
            entity.Property(e => e.Discounts).HasColumnName("discounts");
            entity.Property(e => e.NetTotal).HasColumnName("net_total");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status").HasConversion<string>();
            entity.Property(e => e.Observations).HasColumnName("observations");
            entity.Property(e => e.RegisteredBy).HasColumnName("registered_by");
            entity.Property(e => e.RegisteredAt).HasColumnName("registered_at");
        });

        modelBuilder.Entity<TransactionAnimalDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AnimalId).HasColumnName("animal_id");
            entity.Property(e => e.PricePerKilo).HasColumnName("price_per_kilo");
            entity.Property(e => e.WeightAtNegotiation).HasColumnName("weight_at_negotiation");
            entity.Property(e => e.BaseHeadPrice).HasColumnName("base_head_price");
            entity.Property(e => e.CommissionCost).HasColumnName("commission_cost");
            entity.Property(e => e.TransportCost).HasColumnName("transport_cost");
            entity.Property(e => e.FinalLineValue).HasColumnName("final_line_value");
            entity.Property(e => e.AnimalMovementId).HasColumnName("animal_movement_id");

            entity.HasOne(d => d.Transaction)
                  .WithMany(p => p.AnimalDetails)
                  .HasForeignKey(d => d.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TransactionProductDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");
            entity.Property(e => e.LineSubtotal).HasColumnName("line_subtotal");
            entity.Property(e => e.InventoryMovementId).HasColumnName("inventory_movement_id");

            entity.HasOne(d => d.Transaction)
                  .WithMany(p => p.ProductDetails)
                  .HasForeignKey(d => d.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ThirdParty>(entity =>
        {
            entity.ToTable("third_parties");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.FarmId).HasColumnName("farm_id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.IdentityDocument).HasColumnName("identity_document");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsSupplier).HasColumnName("is_supplier");
            entity.Property(e => e.IsCustomer).HasColumnName("is_customer");
            entity.Property(e => e.IsEmployee).HasColumnName("is_employee");
            entity.Property(e => e.IsVeterinarian).HasColumnName("is_veterinarian");
            entity.Property(e => e.Address).HasColumnName("address");

            entity.HasIndex(e => new { e.FarmId, e.IdentityDocument }).IsUnique();
        });
    }
}
