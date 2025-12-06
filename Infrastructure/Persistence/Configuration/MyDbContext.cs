using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configuration;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> animals { get; set; }

    public virtual DbSet<AnimalCategory> animal_categories { get; set; }

    public virtual DbSet<AnimalMovement> animal_movements { get; set; }

    public virtual DbSet<AuditLogEntry> audit_log_entries { get; set; }

    public virtual DbSet<Batch> batches { get; set; }

    public virtual DbSet<Breed> breeds { get; set; }

    public virtual DbSet<Bucket> buckets { get; set; }

    public virtual DbSet<BucketsAnalytic> buckets_analytics { get; set; }

    public virtual DbSet<BucketsVector> buckets_vectors { get; set; }

    public virtual DbSet<Calving> calvings { get; set; }

    public virtual DbSet<CalvingCalf> calving_calves { get; set; }

    public virtual DbSet<CommercialTransaction> commercial_transactions { get; set; }

    public virtual DbSet<Diet> diets { get; set; }

    public virtual DbSet<DietDetail> diet_details { get; set; }

    public virtual DbSet<Disease> diseases { get; set; }

    public virtual DbSet<Farm> farms { get; set; }

    public virtual DbSet<FeedingEvent> feeding_events { get; set; }

    public virtual DbSet<FlowState> flow_states { get; set; }

    public virtual DbSet<HealthEvent> health_events { get; set; }

    public virtual DbSet<HealthEventDetail> health_event_details { get; set; }

    public virtual DbSet<Identity> identities { get; set; }

    public virtual DbSet<Instance> instances { get; set; }

    public virtual DbSet<InventoryMovement> inventory_movements { get; set; }

    public virtual DbSet<MfaAmrClaim> mfa_amr_claims { get; set; }

    public virtual DbSet<MfaChallenge> mfa_challenges { get; set; }

    public virtual DbSet<MfaFactor> mfa_factors { get; set; }

    public virtual DbSet<Migration> migrations { get; set; }

    public virtual DbSet<MilkProduction> milk_productions { get; set; }

    public virtual DbSet<MovementType> movement_types { get; set; }

    public virtual DbSet<OauthAuthorization> oauth_authorizations { get; set; }

    public virtual DbSet<OauthClient> oauth_clients { get; set; }

    public virtual DbSet<OauthConsent> oauth_consents { get; set; }

    public virtual DbSet<object> Objects { get; set; }

    public virtual DbSet<OneTimeToken> one_time_tokens { get; set; }

    public virtual DbSet<Paddock> paddocks { get; set; }

    public virtual DbSet<Permission> permissions { get; set; }

    public virtual DbSet<Prefix> prefixes { get; set; }

    public virtual DbSet<Product> products { get; set; }

    public virtual DbSet<RefreshToken> refresh_tokens { get; set; }

    public virtual DbSet<ReproductionEvent> reproduction_events { get; set; }

    public virtual DbSet<Role> roles { get; set; }

    public virtual DbSet<S3MultipartUpload> s3_multipart_uploads { get; set; }

    public virtual DbSet<S3MultipartUploadsPart> s3_multipart_uploads_parts { get; set; }

    public virtual DbSet<SamlProvider> saml_providers { get; set; }

    public virtual DbSet<SamlRelayState> saml_relay_states { get; set; }

    public virtual DbSet<SchemaMigration> schema_migrations { get; set; }

    public virtual DbSet<SchemaMigration1> schema_migrations1 { get; set; }

    public virtual DbSet<Session> sessions { get; set; }

    public virtual DbSet<SsoDomain> sso_domains { get; set; }

    public virtual DbSet<SsoProvider> sso_providers { get; set; }

    public virtual DbSet<Subscription> subscriptions { get; set; }

    public virtual DbSet<ThirdParty> third_parties { get; set; }

    public virtual DbSet<TransactionAnimalDetail> transaction_animal_details { get; set; }

    public virtual DbSet<TransactionProductDetail> transaction_product_details { get; set; }

    public virtual DbSet<User> users { get; set; }

    public virtual DbSet<User1> users1 { get; set; }

    public virtual DbSet<UserFarmRole> user_farm_roles { get; set; }

    public virtual DbSet<VLowStockAlert> v_low_stock_alerts { get; set; }

    public virtual DbSet<VectorIndex> vector_indexes { get; set; }

    public virtual DbSet<Weighing> weighings { get; set; }

    public virtual DbSet<WithdrawalPeriod> withdrawal_periods { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.fidfxmajafqhyotqxblf;Password=Nikol2024!postgres;Ssl Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("animals_pkey");

            entity.HasIndex(e => new { e.BirthDate, e.CategoryId }, "idx_animals_birth_category").HasFilter("((current_status)::text = 'ACTIVE'::text)");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CurrentStatus).HasDefaultValueSql("'ACTIVE'::character varying");
            entity.Property(e => e.EntryDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.InitialCost).HasDefaultValueSql("0");
            entity.Property(e => e.Purpose).HasDefaultValueSql("'MEAT'::character varying");

            entity.HasOne(d => d.Batch).WithMany(p => p.Animals).HasConstraintName("animals_batch_id_fkey");

            entity.HasOne(d => d.Breed).WithMany(p => p.Animals).HasConstraintName("animals_breed_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Animals).HasConstraintName("animals_category_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.Animals).HasConstraintName("animals_farm_id_fkey");

            entity.HasOne(d => d.Father).WithMany(p => p.Inversefather).HasConstraintName("animals_father_id_fkey");

            entity.HasOne(d => d.Mother).WithMany(p => p.Inversemother).HasConstraintName("animals_mother_id_fkey");

            entity.HasOne(d => d.Paddock).WithMany(p => p.Animals).HasConstraintName("animals_paddock_id_fkey");
        });

        modelBuilder.Entity<AnimalCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("animal_categories_pkey");

            entity.Property(e => e.MinAgeMonths).HasDefaultValue(0);
        });

        modelBuilder.Entity<AnimalMovement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("animal_movements_pkey");

            entity.Property(e => e.MovementDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.TransactionValue).HasDefaultValueSql("0");

            entity.HasOne(d => d.Animal).WithMany(p => p.AnimalMovements).HasConstraintName("animal_movements_animal_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.AnimalMovements).HasConstraintName("animal_movements_farm_id_fkey");

            entity.HasOne(d => d.MovementType).WithMany(p => p.AnimalMovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("animal_movements_movement_type_id_fkey");

            entity.HasOne(d => d.NewBatch).WithMany(p => p.AnimalMovementnewBatches).HasConstraintName("animal_movements_new_batch_id_fkey");

            entity.HasOne(d => d.NewPaddock).WithMany(p => p.AnimalMovementnewPaddocks).HasConstraintName("animal_movements_new_paddock_id_fkey");

            entity.HasOne(d => d.PreviousBatch).WithMany(p => p.AnimalMovementpreviousBatches).HasConstraintName("animal_movements_previous_batch_id_fkey");

            entity.HasOne(d => d.PreviousPaddock).WithMany(p => p.AnimalMovementpreviousPaddocks).HasConstraintName("animal_movements_previous_paddock_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.AnimalMovements).HasConstraintName("animal_movements_registered_by_fkey");

            entity.HasOne(d => d.ThirdParty).WithMany(p => p.AnimalMovements).HasConstraintName("animal_movements_third_party_id_fkey");
        });

        modelBuilder.Entity<AuditLogEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_log_entries_pkey");

            entity.ToTable("audit_log_entries", "auth", tb => tb.HasComment("Auth: Audit trail for User actions."));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IpAddress).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<Batch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("batches_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Farm).WithMany(p => p.Batches).HasConstraintName("batches_farm_id_fkey");
        });

        modelBuilder.Entity<Breed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("breeds_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<Bucket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buckets_pkey");

            entity.Property(e => e.Public).HasDefaultValue(false);
            entity.Property(e => e.AvifAutodetection).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Owner).HasComment("Field is deprecated, use owner_id instead");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<BucketsAnalytic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buckets_analytics_pkey");

            entity.HasIndex(e => e.Name, "buckets_analytics_unique_name_idx")
                .IsUnique()
                .HasFilter("(deleted_at IS NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Format).HasDefaultValueSql("'ICEBERG'::text");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<BucketsVector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("buckets_vectors_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Calving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("calvings_pkey");

            entity.Property(e => e.CalvingDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.CalvingType).HasDefaultValueSql("'NORMAL'::character varying");
            entity.Property(e => e.NumberOfCalves).HasDefaultValue(1);
            entity.Property(e => e.PlacentaRetention).HasDefaultValue(false);
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Farm).WithMany(p => p.Calvings).HasConstraintName("calvings_farm_id_fkey");

            entity.HasOne(d => d.Mother).WithMany(p => p.Calvings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calvings_mother_id_fkey");
        });

        modelBuilder.Entity<CalvingCalf>(entity =>
        {
            entity.HasKey(e => new { e.CalvingId, e.CalfId }).HasName("calving_calves_pkey");

            entity.Property(e => e.BirthStatus).HasDefaultValueSql("'ALIVE'::character varying");

            entity.HasOne(d => d.Calf).WithMany(p => p.CalvingCalves).HasConstraintName("calving_calves_calf_id_fkey");

            entity.HasOne(d => d.Calving).WithMany(p => p.CalvingCalves).HasConstraintName("calving_calves_calving_id_fkey");
        });

        modelBuilder.Entity<CommercialTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("commercial_transactions_pkey");

            entity.Property(e => e.Discounts).HasDefaultValueSql("0");
            entity.Property(e => e.PaymentStatus).HasDefaultValueSql("'PENDING'::character varying");
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Taxes).HasDefaultValueSql("0");
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.Farm).WithMany(p => p.CommercialTransactions).HasConstraintName("commercial_transactions_farm_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.CommercialTransactions).HasConstraintName("commercial_transactions_registered_by_fkey");

            entity.HasOne(d => d.ThirdParty).WithMany(p => p.CommercialTransactions).HasConstraintName("commercial_transactions_third_party_id_fkey");
        });

        modelBuilder.Entity<Diet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("diets_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Farm).WithMany(p => p.Diets).HasConstraintName("diets_farm_id_fkey");
        });

        modelBuilder.Entity<DietDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("diet_details_pkey");

            entity.Property(e => e.Frequency).HasDefaultValueSql("'DAILY'::character varying");

            entity.HasOne(d => d.Diet).WithMany(p => p.DietDetails).HasConstraintName("diet_details_diet_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.DietDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("diet_details_product_id_fkey");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("diseases_pkey");
        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("farms_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<FeedingEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feeding_events_pkey");

            entity.Property(e => e.SupplyDate).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.Animal).WithMany(p => p.FeedingEvents).HasConstraintName("feeding_events_animal_id_fkey");

            entity.HasOne(d => d.Batch).WithMany(p => p.FeedingEvents).HasConstraintName("feeding_events_batch_id_fkey");

            entity.HasOne(d => d.Diet).WithMany(p => p.FeedingEvents).HasConstraintName("feeding_events_diet_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.FeedingEvents).HasConstraintName("feeding_events_farm_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.FeedingEvents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feeding_events_product_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.FeedingEvents).HasConstraintName("feeding_events_registered_by_fkey");
        });

        modelBuilder.Entity<FlowState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("flow_state_pkey");

            entity.ToTable("FlowState", "auth", tb => tb.HasComment("stores metadata for pkce logins"));

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<HealthEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("health_events_pkey");

            entity.Property(e => e.EventDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.ServiceCost).HasDefaultValueSql("0");

            entity.HasOne(d => d.Animal).WithMany(p => p.HealthEvents).HasConstraintName("health_events_animal_id_fkey");

            entity.HasOne(d => d.Batch).WithMany(p => p.HealthEvents).HasConstraintName("health_events_batch_id_fkey");

            entity.HasOne(d => d.DiseaseDiagnosis).WithMany(p => p.HealthEvents).HasConstraintName("health_events_disease_diagnosis_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.HealthEvents).HasConstraintName("health_events_farm_id_fkey");

            entity.HasOne(d => d.Professional).WithMany(p => p.HealthEvents).HasConstraintName("health_events_professional_id_fkey");
        });

        modelBuilder.Entity<HealthEventDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("health_event_details_pkey");

            entity.HasOne(d => d.HealthEvent).WithMany(p => p.HealthEventDetails).HasConstraintName("health_event_details_health_event_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.HealthEventDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("health_event_details_product_id_fkey");
        });

        modelBuilder.Entity<Identity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("identities_pkey");

            entity.ToTable("identities", "auth", tb => tb.HasComment("Auth: Stores identities associated to a User."));

            entity.HasIndex(e => e.Email, "identities_email_idx").HasOperators(new[] { "text_pattern_ops" });

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Email)
                .HasComputedColumnSql("lower((identity_data ->> 'email'::text))", true)
                .HasComment("Auth: Email is a generated column that references the optional email property in the identity_data");

            entity.HasOne(d => d.User).WithMany(p => p.Identities).HasConstraintName("identities_user_id_fkey");
        });

        modelBuilder.Entity<Instance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("instances_pkey");

            entity.ToTable("instances", "auth", tb => tb.HasComment("Auth: Manages users across multiple sites."));

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inventory_movements_pkey");

            entity.Property(e => e.MovementDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Farm).WithMany(p => p.InventoryMovements).HasConstraintName("inventory_movements_farm_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryMovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inventory_movements_product_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.InventoryMovements).HasConstraintName("inventory_movements_registered_by_fkey");

            entity.HasOne(d => d.ThirdParty).WithMany(p => p.InventoryMovements).HasConstraintName("inventory_movements_third_party_id_fkey");
        });

        modelBuilder.Entity<MfaAmrClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("amr_id_pk");

            entity.ToTable("mfa_amr_claims", "auth", tb => tb.HasComment("auth: stores authenticator method reference claims for multi factor authentication"));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Session).WithMany(p => p.MfaAmrClaims).HasConstraintName("mfa_amr_claims_session_id_fkey");
        });

        modelBuilder.Entity<MfaChallenge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mfa_challenges_pkey");

            entity.ToTable("mfa_challenges", "auth", tb => tb.HasComment("auth: stores metadata about challenge requests made"));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Factor).WithMany(p => p.MfaChallenges).HasConstraintName("mfa_challenges_auth_factor_id_fkey");
        });

        modelBuilder.Entity<MfaFactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mfa_factors_pkey");

            entity.ToTable("mfa_factors", "auth", tb => tb.HasComment("auth: stores metadata about factors"));

            entity.HasIndex(e => new { e.FriendlyName, e.UserId }, "mfa_factors_user_friendly_name_unique")
                .IsUnique()
                .HasFilter("(TRIM(BOTH FROM friendly_name) <> ''::text)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastWebauthnChallengeData).HasComment("Stores the latest WebAuthn challenge data including attestation/assertion for customer verification");

            entity.HasOne(d => d.User).WithMany(p => p.MfaFactors).HasConstraintName("mfa_factors_user_id_fkey");
        });

        modelBuilder.Entity<Migration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("migrations_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExecutedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<MilkProduction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milk_production_pkey");

            entity.Property(e => e.MilkingDate).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Shift).HasDefaultValueSql("'AM'::character varying");

            entity.HasOne(d => d.Animal).WithMany(p => p.MilkProductions).HasConstraintName("milk_production_animal_id_fkey");

            entity.HasOne(d => d.Batch).WithMany(p => p.MilkProductions).HasConstraintName("milk_production_batch_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.MilkProductions).HasConstraintName("milk_production_farm_id_fkey");
        });

        modelBuilder.Entity<MovementType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movement_types_pkey");

            entity.Property(e => e.AffectsInventory).HasDefaultValue(false);
            entity.Property(e => e.InventorySign).HasDefaultValue(0);
        });

        modelBuilder.Entity<OauthAuthorization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_authorizations_pkey");

            entity.HasIndex(e => e.ExpiresAt, "oauth_auth_pending_exp_idx").HasFilter("(status = 'pending'::auth.oauth_authorization_status)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ExpiresAt).HasDefaultValueSql("(now() + '00:03:00'::interval)");

            entity.HasOne(d => d.Client).WithMany(p => p.OauthAuthorizations).HasConstraintName("oauth_authorizations_client_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.OauthAuthorizations)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("oauth_authorizations_user_id_fkey");
        });

        modelBuilder.Entity<OauthClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_clients_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<OauthConsent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oauth_consents_pkey");

            entity.HasIndex(e => e.ClientId, "oauth_consents_active_client_idx").HasFilter("(revoked_at IS NULL)");

            entity.HasIndex(e => new { e.UserId, e.ClientId }, "oauth_consents_active_user_client_idx").HasFilter("(revoked_at IS NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.GrantedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Client).WithMany(p => p.OauthConsents).HasConstraintName("oauth_consents_client_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.OauthConsents).HasConstraintName("oauth_consents_user_id_fkey");
        });

        modelBuilder.Entity<Objects>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("objects_pkey");

            entity.HasIndex(e => new { e.Name, e.BucketId, e.Level }, "idx_name_bucket_level_unique")
                .IsUnique()
                .UseCollation(new[] { "C", null, null });

            entity.HasIndex(e => new { e.BucketId, e.Name }, "idx_objects_bucket_id_name").UseCollation(new[] { null, "C" });

            entity.HasIndex(e => e.Name, "name_prefix_search").HasOperators(new[] { "text_pattern_ops" });

            entity.HasIndex(e => new { e.BucketId, e.Level, e.Name }, "objects_bucket_id_level_idx")
                .IsUnique()
                .UseCollation(new[] { null, null, "C" });

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.LastAccessedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Owner).HasComment("Field is deprecated, use owner_id instead");
            entity.Property(e => e.PathTokens).HasComputedColumnSql("string_to_array(name, '/'::text)", true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Bucket).WithMany(p => p.Objects).HasConstraintName("objects_bucketId_fkey");
        });

        modelBuilder.Entity<OneTimeToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("one_time_tokens_pkey");

            entity.HasIndex(e => e.RelatesTo, "one_time_tokens_relates_to_hash_idx").HasMethod("hash");

            entity.HasIndex(e => e.TokenHash, "one_time_tokens_token_hash_hash_idx").HasMethod("hash");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithMany(p => p.OneTimeTokens).HasConstraintName("one_time_tokens_user_id_fkey");
        });

        modelBuilder.Entity<Paddock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("paddocks_pkey");

            entity.Property(e => e.CurrentStatus).HasDefaultValueSql("'AVAILABLE'::character varying");

            entity.HasOne(d => d.Farm).WithMany(p => p.Paddocks).HasConstraintName("paddocks_farm_id_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");
        });

        modelBuilder.Entity<Prefix>(entity =>
        {
            entity.HasKey(e => new { e.BucketId, e.Level, e.Name }).HasName("prefixes_pkey");

            entity.Property(e => e.Level).HasComputedColumnSql("storage.get_level(name)", true);
            entity.Property(e => e.Name).UseCollation("C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Bucket).WithMany(p => p.Prefixes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prefixes_bucketId_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.AverageCost).HasDefaultValueSql("0");
            entity.Property(e => e.CurrentQuantity).HasDefaultValueSql("0");
            entity.Property(e => e.MinimumStock).HasDefaultValueSql("0");

            entity.HasOne(d => d.Farm).WithMany(p => p.Products).HasConstraintName("products_farm_id_fkey");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens", "auth", tb => tb.HasComment("Auth: Store of tokens used to refresh JWT tokens once they expire."));

            entity.HasOne(d => d.Session).WithMany(p => p.RefreshTokens)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_session_id_fkey");
        });

        modelBuilder.Entity<ReproductionEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reproduction_events_pkey");

            entity.Property(e => e.EventCost).HasDefaultValueSql("0");
            entity.Property(e => e.EventDate).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.Animal).WithMany(p => p.ReproductionEventanimals).HasConstraintName("reproduction_events_animal_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.ReproductionEvents).HasConstraintName("reproduction_events_farm_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.ReproductionEvents).HasConstraintName("reproduction_events_registered_by_fkey");

            entity.HasOne(d => d.Reproducer).WithMany(p => p.ReproductionEventreproducers).HasConstraintName("reproduction_events_reproducer_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("permission_id")
                        .HasConstraintName("role_permissions_permission_id_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("role_id")
                        .HasConstraintName("role_permissions_role_id_fkey"),
                    j =>
                    {
                        j.HasKey("role_id", "permission_id").HasName("role_permissions_pkey");
                        j.ToTable("role_permissions");
                    });
        });

        modelBuilder.Entity<S3MultipartUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s3_multipart_uploads_pkey");

            entity.HasIndex(e => new { e.BucketId, e.Key, e.CreatedAt }, "idx_multipart_uploads_list").UseCollation(new[] { null, "C", null });

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.InProgressSize).HasDefaultValue(0L);
            entity.Property(e => e.Key).UseCollation("C");

            entity.HasOne(d => d.Bucket).WithMany(p => p.S3MultipartUploads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_bucket_id_fkey");
        });

        modelBuilder.Entity<S3MultipartUploadsPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("s3_multipart_uploads_parts_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Key).UseCollation("C");
            entity.Property(e => e.Size).HasDefaultValue(0L);

            entity.HasOne(d => d.Bucket).WithMany(p => p.S3MultipartUploadsParts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_parts_bucket_id_fkey");

            entity.HasOne(d => d.Upload).WithMany(p => p.S3MultipartUploadsParts).HasConstraintName("s3_multipart_uploads_parts_upload_id_fkey");
        });

        modelBuilder.Entity<SamlProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saml_providers_pkey");

            entity.ToTable("saml_providers", "auth", tb => tb.HasComment("Auth: Manages SAML Identity Provider connections."));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SamlProviders).HasConstraintName("saml_providers_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SamlRelayState>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saml_relay_states_pkey");

            entity.ToTable("saml_relay_states", "auth", tb => tb.HasComment("Auth: Contains SAML Relay State information for each Service Provider initiated login."));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FlowState).WithMany(p => p.SamlRelayStates)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("saml_relay_states_flow_state_id_fkey");

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SamlRelayStates).HasConstraintName("saml_relay_states_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SchemaMigration>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("schema_migrations_pkey");

            entity.ToTable("schema_migrations", "auth", tb => tb.HasComment("Auth: Manages updates to the auth system."));
        });

        modelBuilder.Entity<SchemaMigration1>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("schema_migrations_pkey");

            entity.Property(e => e.Version).ValueGeneratedNever();
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessions_pkey");

            entity.ToTable("sessions", "auth", tb => tb.HasComment("Auth: Stores Session data associated to a User."));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NotAfter).HasComment("Auth: Not after is a nullable column that contains a timestamp after which the Session should be regarded as expired.");
            entity.Property(e => e.RefreshTokenCounter).HasComment("Holds the ID (counter) of the last issued refresh token.");
            entity.Property(e => e.RefreshTokenHmacKey).HasComment("Holds a HMAC-SHA256 key used to sign refresh tokens for this Session.");

            entity.HasOne(d => d.OauthClient).WithMany(p => p.Sessions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sessions_oauth_client_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions).HasConstraintName("sessions_user_id_fkey");
        });

        modelBuilder.Entity<SsoDomain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sso_domains_pkey");

            entity.ToTable("sso_domains", "auth", tb => tb.HasComment("Auth: Manages SSO email address domain mapping to an SSO Identity Provider."));

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SsoProvider).WithMany(p => p.SsoDomains).HasConstraintName("sso_domains_sso_provider_id_fkey");
        });

        modelBuilder.Entity<SsoProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sso_providers_pkey");

            entity.ToTable("sso_providers", "auth", tb => tb.HasComment("Auth: Manages SSO Identity provider information; see saml_providers for SAML."));

            entity.HasIndex(e => e.ResourceId, "sso_providers_resource_id_pattern_idx").HasOperators(new[] { "text_pattern_ops" });

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ResourceId).HasComment("Auth: Uniquely identifies a SSO provider according to a User-chosen resource ID (case insensitive), useful in infrastructure as code.");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_subscription");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('utc'::text, now())");
        });

        modelBuilder.Entity<ThirdParty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("third_parties_pkey");

            entity.Property(e => e.IsCustomer).HasDefaultValue(false);
            entity.Property(e => e.IsEmployee).HasDefaultValue(false);
            entity.Property(e => e.IsSupplier).HasDefaultValue(false);
            entity.Property(e => e.IsVeterinarian).HasDefaultValue(false);

            entity.HasOne(d => d.Farm).WithMany(p => p.ThirdParties).HasConstraintName("third_parties_farm_id_fkey");
        });

        modelBuilder.Entity<TransactionAnimalDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_animal_details_pkey");

            entity.Property(e => e.CommissionCost).HasDefaultValueSql("0");
            entity.Property(e => e.TransportCost).HasDefaultValueSql("0");

            entity.HasOne(d => d.Animal).WithMany(p => p.TransactionAnimalDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_animal_details_animal_id_fkey");

            entity.HasOne(d => d.AnimalMovement).WithMany(p => p.TransactionAnimalDetails).HasConstraintName("transaction_animal_details_animal_movement_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionAnimalDetails).HasConstraintName("transaction_animal_details_transaction_id_fkey");
        });

        modelBuilder.Entity<TransactionProductDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_product_details_pkey");

            entity.HasOne(d => d.InventoryMovement).WithMany(p => p.TransactionProductDetails).HasConstraintName("transaction_product_details_inventory_movement_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionProductDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_product_details_product_id_fkey");

            entity.HasOne(d => d.Transaction).WithMany(p => p.TransactionProductDetails).HasConstraintName("transaction_product_details_transaction_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<User1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "auth", tb => tb.HasComment("Auth: Stores User login data within a secure schema."));

            entity.HasIndex(e => e.ConfirmationToken, "confirmation_token_idx")
                .IsUnique()
                .HasFilter("((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.EmailChangeTokenCurrent, "email_change_token_current_idx")
                .IsUnique()
                .HasFilter("((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.EmailChangeTokenNew, "email_change_token_new_idx")
                .IsUnique()
                .HasFilter("((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.ReauthenticationToken, "reauthentication_token_idx")
                .IsUnique()
                .HasFilter("((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.RecoveryToken, "recovery_token_idx")
                .IsUnique()
                .HasFilter("((recovery_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.Email, "users_email_partial_key")
                .IsUnique()
                .HasFilter("(is_sso_user = false)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConfirmedAt).HasComputedColumnSql("LEAST(email_confirmed_at, phone_confirmed_at)", true);
            entity.Property(e => e.EmailChangeConfirmStatus).HasDefaultValue((short)0);
            entity.Property(e => e.EmailChangeTokenCurrent).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IsAnonymous).HasDefaultValue(false);
            entity.Property(e => e.IsSsoUser)
                .HasDefaultValue(false)
                .HasComment("Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails.");
            entity.Property(e => e.Phone).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.PhoneChange).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PhoneChangeToken).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ReauthenticationToken).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<UserFarmRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_farm_role_pkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.UserFarmRoles).HasConstraintName("user_farm_role_farm_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.UserFarmRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_farm_role_role_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserFarmRoles).HasConstraintName("user_farm_role_user_id_fkey");
        });

        modelBuilder.Entity<VLowStockAlert>(entity =>
        {
            entity.ToView("v_low_stock_alerts");
        });

        modelBuilder.Entity<VectorIndex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("vector_indexes_pkey");

            entity.HasIndex(e => new { e.Name, e.BucketId }, "vector_indexes_name_bucket_id_idx")
                .IsUnique()
                .UseCollation(new[] { "C", null });

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.Name).UseCollation("C");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Bucket).WithMany(p => p.VectorIndices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vector_indexes_bucket_id_fkey");
        });

        modelBuilder.Entity<Weighing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("weighings_pkey");

            entity.Property(e => e.WeighingDate).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.Animal).WithMany(p => p.Weighings).HasConstraintName("weighings_animal_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.Weighings).HasConstraintName("weighings_farm_id_fkey");

            entity.HasOne(d => d.RegisteredBynavigation).WithMany(p => p.Weighings).HasConstraintName("weighings_registered_by_fkey");
        });

        modelBuilder.Entity<WithdrawalPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("withdrawal_periods_pkey");

            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(d => d.Animal).WithMany(p => p.WithdrawalPeriods).HasConstraintName("withdrawal_periods_animal_id_fkey");

            entity.HasOne(d => d.Farm).WithMany(p => p.WithdrawalPeriods).HasConstraintName("withdrawal_periods_farm_id_fkey");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
