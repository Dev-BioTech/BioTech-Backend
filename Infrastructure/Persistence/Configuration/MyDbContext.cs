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

    public virtual DbSet<animal> animals { get; set; }

    public virtual DbSet<animal_category> animal_categories { get; set; }

    public virtual DbSet<animal_movement> animal_movements { get; set; }

    public virtual DbSet<audit_log_entry> audit_log_entries { get; set; }

    public virtual DbSet<batch> batches { get; set; }

    public virtual DbSet<breed> breeds { get; set; }

    public virtual DbSet<bucket> buckets { get; set; }

    public virtual DbSet<buckets_analytic> buckets_analytics { get; set; }

    public virtual DbSet<buckets_vector> buckets_vectors { get; set; }

    public virtual DbSet<calving> calvings { get; set; }

    public virtual DbSet<calving_calf> calving_calves { get; set; }

    public virtual DbSet<commercial_transaction> commercial_transactions { get; set; }

    public virtual DbSet<diet> diets { get; set; }

    public virtual DbSet<diet_detail> diet_details { get; set; }

    public virtual DbSet<disease> diseases { get; set; }

    public virtual DbSet<farm> farms { get; set; }

    public virtual DbSet<feeding_event> feeding_events { get; set; }

    public virtual DbSet<flow_state> flow_states { get; set; }

    public virtual DbSet<health_event> health_events { get; set; }

    public virtual DbSet<health_event_detail> health_event_details { get; set; }

    public virtual DbSet<identity> identities { get; set; }

    public virtual DbSet<instance> instances { get; set; }

    public virtual DbSet<inventory_movement> inventory_movements { get; set; }

    public virtual DbSet<mfa_amr_claim> mfa_amr_claims { get; set; }

    public virtual DbSet<mfa_challenge> mfa_challenges { get; set; }

    public virtual DbSet<mfa_factor> mfa_factors { get; set; }

    public virtual DbSet<migration> migrations { get; set; }

    public virtual DbSet<milk_production> milk_productions { get; set; }

    public virtual DbSet<movement_type> movement_types { get; set; }

    public virtual DbSet<oauth_authorization> oauth_authorizations { get; set; }

    public virtual DbSet<oauth_client> oauth_clients { get; set; }

    public virtual DbSet<oauth_consent> oauth_consents { get; set; }

    public virtual DbSet<object> objects { get; set; }

    public virtual DbSet<one_time_token> one_time_tokens { get; set; }

    public virtual DbSet<paddock> paddocks { get; set; }

    public virtual DbSet<permission> permissions { get; set; }

    public virtual DbSet<prefix> prefixes { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<refresh_token> refresh_tokens { get; set; }

    public virtual DbSet<reproduction_event> reproduction_events { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<s3_multipart_upload> s3_multipart_uploads { get; set; }

    public virtual DbSet<s3_multipart_uploads_part> s3_multipart_uploads_parts { get; set; }

    public virtual DbSet<saml_provider> saml_providers { get; set; }

    public virtual DbSet<saml_relay_state> saml_relay_states { get; set; }

    public virtual DbSet<schema_migration> schema_migrations { get; set; }

    public virtual DbSet<schema_migration1> schema_migrations1 { get; set; }

    public virtual DbSet<session> sessions { get; set; }

    public virtual DbSet<sso_domain> sso_domains { get; set; }

    public virtual DbSet<sso_provider> sso_providers { get; set; }

    public virtual DbSet<subscription> subscriptions { get; set; }

    public virtual DbSet<third_party> third_parties { get; set; }

    public virtual DbSet<transaction_animal_detail> transaction_animal_details { get; set; }

    public virtual DbSet<transaction_product_detail> transaction_product_details { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<user1> users1 { get; set; }

    public virtual DbSet<user_farm_role> user_farm_roles { get; set; }

    public virtual DbSet<v_low_stock_alert> v_low_stock_alerts { get; set; }

    public virtual DbSet<vector_index> vector_indexes { get; set; }

    public virtual DbSet<weighing> weighings { get; set; }

    public virtual DbSet<withdrawal_period> withdrawal_periods { get; set; }

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

        modelBuilder.Entity<animal>(entity =>
        {
            entity.HasKey(e => e.id).HasName("animals_pkey");

            entity.HasIndex(e => new { e.birth_date, e.category_id }, "idx_animals_birth_category").HasFilter("((current_status)::text = 'ACTIVE'::text)");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.current_status).HasDefaultValueSql("'ACTIVE'::character varying");
            entity.Property(e => e.entry_date).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.initial_cost).HasDefaultValueSql("0");
            entity.Property(e => e.purpose).HasDefaultValueSql("'MEAT'::character varying");

            entity.HasOne(d => d.batch).WithMany(p => p.animals).HasConstraintName("animals_batch_id_fkey");

            entity.HasOne(d => d.breed).WithMany(p => p.animals).HasConstraintName("animals_breed_id_fkey");

            entity.HasOne(d => d.category).WithMany(p => p.animals).HasConstraintName("animals_category_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.animals).HasConstraintName("animals_farm_id_fkey");

            entity.HasOne(d => d.father).WithMany(p => p.Inversefather).HasConstraintName("animals_father_id_fkey");

            entity.HasOne(d => d.mother).WithMany(p => p.Inversemother).HasConstraintName("animals_mother_id_fkey");

            entity.HasOne(d => d.paddock).WithMany(p => p.animals).HasConstraintName("animals_paddock_id_fkey");
        });

        modelBuilder.Entity<animal_category>(entity =>
        {
            entity.HasKey(e => e.id).HasName("animal_categories_pkey");

            entity.Property(e => e.min_age_months).HasDefaultValue(0);
        });

        modelBuilder.Entity<animal_movement>(entity =>
        {
            entity.HasKey(e => e.id).HasName("animal_movements_pkey");

            entity.Property(e => e.movement_date).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.transaction_value).HasDefaultValueSql("0");

            entity.HasOne(d => d.animal).WithMany(p => p.animal_movements).HasConstraintName("animal_movements_animal_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.animal_movements).HasConstraintName("animal_movements_farm_id_fkey");

            entity.HasOne(d => d.movement_type).WithMany(p => p.animal_movements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("animal_movements_movement_type_id_fkey");

            entity.HasOne(d => d.new_batch).WithMany(p => p.animal_movementnew_batches).HasConstraintName("animal_movements_new_batch_id_fkey");

            entity.HasOne(d => d.new_paddock).WithMany(p => p.animal_movementnew_paddocks).HasConstraintName("animal_movements_new_paddock_id_fkey");

            entity.HasOne(d => d.previous_batch).WithMany(p => p.animal_movementprevious_batches).HasConstraintName("animal_movements_previous_batch_id_fkey");

            entity.HasOne(d => d.previous_paddock).WithMany(p => p.animal_movementprevious_paddocks).HasConstraintName("animal_movements_previous_paddock_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.animal_movements).HasConstraintName("animal_movements_registered_by_fkey");

            entity.HasOne(d => d.third_party).WithMany(p => p.animal_movements).HasConstraintName("animal_movements_third_party_id_fkey");
        });

        modelBuilder.Entity<audit_log_entry>(entity =>
        {
            entity.HasKey(e => e.id).HasName("audit_log_entries_pkey");

            entity.ToTable("audit_log_entries", "auth", tb => tb.HasComment("Auth: Audit trail for user actions."));

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.ip_address).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<batch>(entity =>
        {
            entity.HasKey(e => e.id).HasName("batches_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.farm).WithMany(p => p.batches).HasConstraintName("batches_farm_id_fkey");
        });

        modelBuilder.Entity<breed>(entity =>
        {
            entity.HasKey(e => e.id).HasName("breeds_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
        });

        modelBuilder.Entity<bucket>(entity =>
        {
            entity.HasKey(e => e.id).HasName("buckets_pkey");

            entity.Property(e => e._public).HasDefaultValue(false);
            entity.Property(e => e.avif_autodetection).HasDefaultValue(false);
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.owner).HasComment("Field is deprecated, use owner_id instead");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<buckets_analytic>(entity =>
        {
            entity.HasKey(e => e.id).HasName("buckets_analytics_pkey");

            entity.HasIndex(e => e.name, "buckets_analytics_unique_name_idx")
                .IsUnique()
                .HasFilter("(deleted_at IS NULL)");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.format).HasDefaultValueSql("'ICEBERG'::text");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<buckets_vector>(entity =>
        {
            entity.HasKey(e => e.id).HasName("buckets_vectors_pkey");

            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<calving>(entity =>
        {
            entity.HasKey(e => e.id).HasName("calvings_pkey");

            entity.Property(e => e.calving_date).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.calving_type).HasDefaultValueSql("'NORMAL'::character varying");
            entity.Property(e => e.number_of_calves).HasDefaultValue(1);
            entity.Property(e => e.placenta_retention).HasDefaultValue(false);
            entity.Property(e => e.registered_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.farm).WithMany(p => p.calvings).HasConstraintName("calvings_farm_id_fkey");

            entity.HasOne(d => d.mother).WithMany(p => p.calvings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calvings_mother_id_fkey");
        });

        modelBuilder.Entity<calving_calf>(entity =>
        {
            entity.HasKey(e => new { e.calving_id, e.calf_id }).HasName("calving_calves_pkey");

            entity.Property(e => e.birth_status).HasDefaultValueSql("'ALIVE'::character varying");

            entity.HasOne(d => d.calf).WithMany(p => p.calving_calves).HasConstraintName("calving_calves_calf_id_fkey");

            entity.HasOne(d => d.calving).WithMany(p => p.calving_calves).HasConstraintName("calving_calves_calving_id_fkey");
        });

        modelBuilder.Entity<commercial_transaction>(entity =>
        {
            entity.HasKey(e => e.id).HasName("commercial_transactions_pkey");

            entity.Property(e => e.discounts).HasDefaultValueSql("0");
            entity.Property(e => e.payment_status).HasDefaultValueSql("'PENDING'::character varying");
            entity.Property(e => e.registered_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.taxes).HasDefaultValueSql("0");
            entity.Property(e => e.transaction_date).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.farm).WithMany(p => p.commercial_transactions).HasConstraintName("commercial_transactions_farm_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.commercial_transactions).HasConstraintName("commercial_transactions_registered_by_fkey");

            entity.HasOne(d => d.third_party).WithMany(p => p.commercial_transactions).HasConstraintName("commercial_transactions_third_party_id_fkey");
        });

        modelBuilder.Entity<diet>(entity =>
        {
            entity.HasKey(e => e.id).HasName("diets_pkey");

            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.farm).WithMany(p => p.diets).HasConstraintName("diets_farm_id_fkey");
        });

        modelBuilder.Entity<diet_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("diet_details_pkey");

            entity.Property(e => e.frequency).HasDefaultValueSql("'DAILY'::character varying");

            entity.HasOne(d => d.diet).WithMany(p => p.diet_details).HasConstraintName("diet_details_diet_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.diet_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("diet_details_product_id_fkey");
        });

        modelBuilder.Entity<disease>(entity =>
        {
            entity.HasKey(e => e.id).HasName("diseases_pkey");
        });

        modelBuilder.Entity<farm>(entity =>
        {
            entity.HasKey(e => e.id).HasName("farms_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<feeding_event>(entity =>
        {
            entity.HasKey(e => e.id).HasName("feeding_events_pkey");

            entity.Property(e => e.supply_date).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.animal).WithMany(p => p.feeding_events).HasConstraintName("feeding_events_animal_id_fkey");

            entity.HasOne(d => d.batch).WithMany(p => p.feeding_events).HasConstraintName("feeding_events_batch_id_fkey");

            entity.HasOne(d => d.diet).WithMany(p => p.feeding_events).HasConstraintName("feeding_events_diet_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.feeding_events).HasConstraintName("feeding_events_farm_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.feeding_events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("feeding_events_product_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.feeding_events).HasConstraintName("feeding_events_registered_by_fkey");
        });

        modelBuilder.Entity<flow_state>(entity =>
        {
            entity.HasKey(e => e.id).HasName("flow_state_pkey");

            entity.ToTable("flow_state", "auth", tb => tb.HasComment("stores metadata for pkce logins"));

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<health_event>(entity =>
        {
            entity.HasKey(e => e.id).HasName("health_events_pkey");

            entity.Property(e => e.event_date).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.service_cost).HasDefaultValueSql("0");

            entity.HasOne(d => d.animal).WithMany(p => p.health_events).HasConstraintName("health_events_animal_id_fkey");

            entity.HasOne(d => d.batch).WithMany(p => p.health_events).HasConstraintName("health_events_batch_id_fkey");

            entity.HasOne(d => d.disease_diagnosis).WithMany(p => p.health_events).HasConstraintName("health_events_disease_diagnosis_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.health_events).HasConstraintName("health_events_farm_id_fkey");

            entity.HasOne(d => d.professional).WithMany(p => p.health_events).HasConstraintName("health_events_professional_id_fkey");
        });

        modelBuilder.Entity<health_event_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("health_event_details_pkey");

            entity.HasOne(d => d.health_event).WithMany(p => p.health_event_details).HasConstraintName("health_event_details_health_event_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.health_event_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("health_event_details_product_id_fkey");
        });

        modelBuilder.Entity<identity>(entity =>
        {
            entity.HasKey(e => e.id).HasName("identities_pkey");

            entity.ToTable("identities", "auth", tb => tb.HasComment("Auth: Stores identities associated to a user."));

            entity.HasIndex(e => e.email, "identities_email_idx").HasOperators(new[] { "text_pattern_ops" });

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.email)
                .HasComputedColumnSql("lower((identity_data ->> 'email'::text))", true)
                .HasComment("Auth: Email is a generated column that references the optional email property in the identity_data");

            entity.HasOne(d => d.user).WithMany(p => p.identities).HasConstraintName("identities_user_id_fkey");
        });

        modelBuilder.Entity<instance>(entity =>
        {
            entity.HasKey(e => e.id).HasName("instances_pkey");

            entity.ToTable("instances", "auth", tb => tb.HasComment("Auth: Manages users across multiple sites."));

            entity.Property(e => e.id).ValueGeneratedNever();
        });

        modelBuilder.Entity<inventory_movement>(entity =>
        {
            entity.HasKey(e => e.id).HasName("inventory_movements_pkey");

            entity.Property(e => e.movement_date).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.farm).WithMany(p => p.inventory_movements).HasConstraintName("inventory_movements_farm_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.inventory_movements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inventory_movements_product_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.inventory_movements).HasConstraintName("inventory_movements_registered_by_fkey");

            entity.HasOne(d => d.third_party).WithMany(p => p.inventory_movements).HasConstraintName("inventory_movements_third_party_id_fkey");
        });

        modelBuilder.Entity<mfa_amr_claim>(entity =>
        {
            entity.HasKey(e => e.id).HasName("amr_id_pk");

            entity.ToTable("mfa_amr_claims", "auth", tb => tb.HasComment("auth: stores authenticator method reference claims for multi factor authentication"));

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.session).WithMany(p => p.mfa_amr_claims).HasConstraintName("mfa_amr_claims_session_id_fkey");
        });

        modelBuilder.Entity<mfa_challenge>(entity =>
        {
            entity.HasKey(e => e.id).HasName("mfa_challenges_pkey");

            entity.ToTable("mfa_challenges", "auth", tb => tb.HasComment("auth: stores metadata about challenge requests made"));

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.factor).WithMany(p => p.mfa_challenges).HasConstraintName("mfa_challenges_auth_factor_id_fkey");
        });

        modelBuilder.Entity<mfa_factor>(entity =>
        {
            entity.HasKey(e => e.id).HasName("mfa_factors_pkey");

            entity.ToTable("mfa_factors", "auth", tb => tb.HasComment("auth: stores metadata about factors"));

            entity.HasIndex(e => new { e.friendly_name, e.user_id }, "mfa_factors_user_friendly_name_unique")
                .IsUnique()
                .HasFilter("(TRIM(BOTH FROM friendly_name) <> ''::text)");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.last_webauthn_challenge_data).HasComment("Stores the latest WebAuthn challenge data including attestation/assertion for customer verification");

            entity.HasOne(d => d.user).WithMany(p => p.mfa_factors).HasConstraintName("mfa_factors_user_id_fkey");
        });

        modelBuilder.Entity<migration>(entity =>
        {
            entity.HasKey(e => e.id).HasName("migrations_pkey");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.executed_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<milk_production>(entity =>
        {
            entity.HasKey(e => e.id).HasName("milk_production_pkey");

            entity.Property(e => e.milking_date).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.shift).HasDefaultValueSql("'AM'::character varying");

            entity.HasOne(d => d.animal).WithMany(p => p.milk_productions).HasConstraintName("milk_production_animal_id_fkey");

            entity.HasOne(d => d.batch).WithMany(p => p.milk_productions).HasConstraintName("milk_production_batch_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.milk_productions).HasConstraintName("milk_production_farm_id_fkey");
        });

        modelBuilder.Entity<movement_type>(entity =>
        {
            entity.HasKey(e => e.id).HasName("movement_types_pkey");

            entity.Property(e => e.affects_inventory).HasDefaultValue(false);
            entity.Property(e => e.inventory_sign).HasDefaultValue(0);
        });

        modelBuilder.Entity<oauth_authorization>(entity =>
        {
            entity.HasKey(e => e.id).HasName("oauth_authorizations_pkey");

            entity.HasIndex(e => e.expires_at, "oauth_auth_pending_exp_idx").HasFilter("(status = 'pending'::auth.oauth_authorization_status)");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.expires_at).HasDefaultValueSql("(now() + '00:03:00'::interval)");

            entity.HasOne(d => d.client).WithMany(p => p.oauth_authorizations).HasConstraintName("oauth_authorizations_client_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.oauth_authorizations)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("oauth_authorizations_user_id_fkey");
        });

        modelBuilder.Entity<oauth_client>(entity =>
        {
            entity.HasKey(e => e.id).HasName("oauth_clients_pkey");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<oauth_consent>(entity =>
        {
            entity.HasKey(e => e.id).HasName("oauth_consents_pkey");

            entity.HasIndex(e => e.client_id, "oauth_consents_active_client_idx").HasFilter("(revoked_at IS NULL)");

            entity.HasIndex(e => new { e.user_id, e.client_id }, "oauth_consents_active_user_client_idx").HasFilter("(revoked_at IS NULL)");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.granted_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.client).WithMany(p => p.oauth_consents).HasConstraintName("oauth_consents_client_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.oauth_consents).HasConstraintName("oauth_consents_user_id_fkey");
        });

        modelBuilder.Entity<objects>(entity =>
        {
            entity.HasKey(e => e.id).HasName("objects_pkey");

            entity.HasIndex(e => new { e.name, e.bucket_id, e.level }, "idx_name_bucket_level_unique")
                .IsUnique()
                .UseCollation(new[] { "C", null, null });

            entity.HasIndex(e => new { e.bucket_id, e.name }, "idx_objects_bucket_id_name").UseCollation(new[] { null, "C" });

            entity.HasIndex(e => e.name, "name_prefix_search").HasOperators(new[] { "text_pattern_ops" });

            entity.HasIndex(e => new { e.bucket_id, e.level, e.name }, "objects_bucket_id_level_idx")
                .IsUnique()
                .UseCollation(new[] { null, null, "C" });

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.last_accessed_at).HasDefaultValueSql("now()");
            entity.Property(e => e.owner).HasComment("Field is deprecated, use owner_id instead");
            entity.Property(e => e.path_tokens).HasComputedColumnSql("string_to_array(name, '/'::text)", true);
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.bucket).WithMany(p => p.objects).HasConstraintName("objects_bucketId_fkey");
        });

        modelBuilder.Entity<one_time_token>(entity =>
        {
            entity.HasKey(e => e.id).HasName("one_time_tokens_pkey");

            entity.HasIndex(e => e.relates_to, "one_time_tokens_relates_to_hash_idx").HasMethod("hash");

            entity.HasIndex(e => e.token_hash, "one_time_tokens_token_hash_hash_idx").HasMethod("hash");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.user).WithMany(p => p.one_time_tokens).HasConstraintName("one_time_tokens_user_id_fkey");
        });

        modelBuilder.Entity<paddock>(entity =>
        {
            entity.HasKey(e => e.id).HasName("paddocks_pkey");

            entity.Property(e => e.current_status).HasDefaultValueSql("'AVAILABLE'::character varying");

            entity.HasOne(d => d.farm).WithMany(p => p.paddocks).HasConstraintName("paddocks_farm_id_fkey");
        });

        modelBuilder.Entity<permission>(entity =>
        {
            entity.HasKey(e => e.id).HasName("permissions_pkey");
        });

        modelBuilder.Entity<prefix>(entity =>
        {
            entity.HasKey(e => new { e.bucket_id, e.level, e.name }).HasName("prefixes_pkey");

            entity.Property(e => e.level).HasComputedColumnSql("storage.get_level(name)", true);
            entity.Property(e => e.name).UseCollation("C");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.bucket).WithMany(p => p.prefixes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("prefixes_bucketId_fkey");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("products_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.average_cost).HasDefaultValueSql("0");
            entity.Property(e => e.current_quantity).HasDefaultValueSql("0");
            entity.Property(e => e.minimum_stock).HasDefaultValueSql("0");

            entity.HasOne(d => d.farm).WithMany(p => p.products).HasConstraintName("products_farm_id_fkey");
        });

        modelBuilder.Entity<refresh_token>(entity =>
        {
            entity.HasKey(e => e.id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens", "auth", tb => tb.HasComment("Auth: Store of tokens used to refresh JWT tokens once they expire."));

            entity.HasOne(d => d.session).WithMany(p => p.refresh_tokens)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_session_id_fkey");
        });

        modelBuilder.Entity<reproduction_event>(entity =>
        {
            entity.HasKey(e => e.id).HasName("reproduction_events_pkey");

            entity.Property(e => e.event_cost).HasDefaultValueSql("0");
            entity.Property(e => e.event_date).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.animal).WithMany(p => p.reproduction_eventanimals).HasConstraintName("reproduction_events_animal_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.reproduction_events).HasConstraintName("reproduction_events_farm_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.reproduction_events).HasConstraintName("reproduction_events_registered_by_fkey");

            entity.HasOne(d => d.reproducer).WithMany(p => p.reproduction_eventreproducers).HasConstraintName("reproduction_events_reproducer_id_fkey");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("roles_pkey");

            entity.HasMany(d => d.permissions).WithMany(p => p.roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permission",
                    r => r.HasOne<permission>().WithMany()
                        .HasForeignKey("permission_id")
                        .HasConstraintName("role_permissions_permission_id_fkey"),
                    l => l.HasOne<role>().WithMany()
                        .HasForeignKey("role_id")
                        .HasConstraintName("role_permissions_role_id_fkey"),
                    j =>
                    {
                        j.HasKey("role_id", "permission_id").HasName("role_permissions_pkey");
                        j.ToTable("role_permissions");
                    });
        });

        modelBuilder.Entity<s3_multipart_upload>(entity =>
        {
            entity.HasKey(e => e.id).HasName("s3_multipart_uploads_pkey");

            entity.HasIndex(e => new { e.bucket_id, e.key, e.created_at }, "idx_multipart_uploads_list").UseCollation(new[] { null, "C", null });

            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.in_progress_size).HasDefaultValue(0L);
            entity.Property(e => e.key).UseCollation("C");

            entity.HasOne(d => d.bucket).WithMany(p => p.s3_multipart_uploads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_bucket_id_fkey");
        });

        modelBuilder.Entity<s3_multipart_uploads_part>(entity =>
        {
            entity.HasKey(e => e.id).HasName("s3_multipart_uploads_parts_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.key).UseCollation("C");
            entity.Property(e => e.size).HasDefaultValue(0L);

            entity.HasOne(d => d.bucket).WithMany(p => p.s3_multipart_uploads_parts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("s3_multipart_uploads_parts_bucket_id_fkey");

            entity.HasOne(d => d.upload).WithMany(p => p.s3_multipart_uploads_parts).HasConstraintName("s3_multipart_uploads_parts_upload_id_fkey");
        });

        modelBuilder.Entity<saml_provider>(entity =>
        {
            entity.HasKey(e => e.id).HasName("saml_providers_pkey");

            entity.ToTable("saml_providers", "auth", tb => tb.HasComment("Auth: Manages SAML Identity Provider connections."));

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.sso_provider).WithMany(p => p.saml_providers).HasConstraintName("saml_providers_sso_provider_id_fkey");
        });

        modelBuilder.Entity<saml_relay_state>(entity =>
        {
            entity.HasKey(e => e.id).HasName("saml_relay_states_pkey");

            entity.ToTable("saml_relay_states", "auth", tb => tb.HasComment("Auth: Contains SAML Relay State information for each Service Provider initiated login."));

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.flow_state).WithMany(p => p.saml_relay_states)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("saml_relay_states_flow_state_id_fkey");

            entity.HasOne(d => d.sso_provider).WithMany(p => p.saml_relay_states).HasConstraintName("saml_relay_states_sso_provider_id_fkey");
        });

        modelBuilder.Entity<schema_migration>(entity =>
        {
            entity.HasKey(e => e.version).HasName("schema_migrations_pkey");

            entity.ToTable("schema_migrations", "auth", tb => tb.HasComment("Auth: Manages updates to the auth system."));
        });

        modelBuilder.Entity<schema_migration1>(entity =>
        {
            entity.HasKey(e => e.version).HasName("schema_migrations_pkey");

            entity.Property(e => e.version).ValueGeneratedNever();
        });

        modelBuilder.Entity<session>(entity =>
        {
            entity.HasKey(e => e.id).HasName("sessions_pkey");

            entity.ToTable("sessions", "auth", tb => tb.HasComment("Auth: Stores session data associated to a user."));

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.not_after).HasComment("Auth: Not after is a nullable column that contains a timestamp after which the session should be regarded as expired.");
            entity.Property(e => e.refresh_token_counter).HasComment("Holds the ID (counter) of the last issued refresh token.");
            entity.Property(e => e.refresh_token_hmac_key).HasComment("Holds a HMAC-SHA256 key used to sign refresh tokens for this session.");

            entity.HasOne(d => d.oauth_client).WithMany(p => p.sessions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sessions_oauth_client_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.sessions).HasConstraintName("sessions_user_id_fkey");
        });

        modelBuilder.Entity<sso_domain>(entity =>
        {
            entity.HasKey(e => e.id).HasName("sso_domains_pkey");

            entity.ToTable("sso_domains", "auth", tb => tb.HasComment("Auth: Manages SSO email address domain mapping to an SSO Identity Provider."));

            entity.Property(e => e.id).ValueGeneratedNever();

            entity.HasOne(d => d.sso_provider).WithMany(p => p.sso_domains).HasConstraintName("sso_domains_sso_provider_id_fkey");
        });

        modelBuilder.Entity<sso_provider>(entity =>
        {
            entity.HasKey(e => e.id).HasName("sso_providers_pkey");

            entity.ToTable("sso_providers", "auth", tb => tb.HasComment("Auth: Manages SSO identity provider information; see saml_providers for SAML."));

            entity.HasIndex(e => e.resource_id, "sso_providers_resource_id_pattern_idx").HasOperators(new[] { "text_pattern_ops" });

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.resource_id).HasComment("Auth: Uniquely identifies a SSO provider according to a user-chosen resource ID (case insensitive), useful in infrastructure as code.");
        });

        modelBuilder.Entity<subscription>(entity =>
        {
            entity.HasKey(e => e.id).HasName("pk_subscription");

            entity.Property(e => e.id).UseIdentityAlwaysColumn();
            entity.Property(e => e.created_at).HasDefaultValueSql("timezone('utc'::text, now())");
        });

        modelBuilder.Entity<third_party>(entity =>
        {
            entity.HasKey(e => e.id).HasName("third_parties_pkey");

            entity.Property(e => e.is_customer).HasDefaultValue(false);
            entity.Property(e => e.is_employee).HasDefaultValue(false);
            entity.Property(e => e.is_supplier).HasDefaultValue(false);
            entity.Property(e => e.is_veterinarian).HasDefaultValue(false);

            entity.HasOne(d => d.farm).WithMany(p => p.third_parties).HasConstraintName("third_parties_farm_id_fkey");
        });

        modelBuilder.Entity<transaction_animal_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("transaction_animal_details_pkey");

            entity.Property(e => e.commission_cost).HasDefaultValueSql("0");
            entity.Property(e => e.transport_cost).HasDefaultValueSql("0");

            entity.HasOne(d => d.animal).WithMany(p => p.transaction_animal_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_animal_details_animal_id_fkey");

            entity.HasOne(d => d.animal_movement).WithMany(p => p.transaction_animal_details).HasConstraintName("transaction_animal_details_animal_movement_id_fkey");

            entity.HasOne(d => d.transaction).WithMany(p => p.transaction_animal_details).HasConstraintName("transaction_animal_details_transaction_id_fkey");
        });

        modelBuilder.Entity<transaction_product_detail>(entity =>
        {
            entity.HasKey(e => e.id).HasName("transaction_product_details_pkey");

            entity.HasOne(d => d.inventory_movement).WithMany(p => p.transaction_product_details).HasConstraintName("transaction_product_details_inventory_movement_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.transaction_product_details)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_product_details_product_id_fkey");

            entity.HasOne(d => d.transaction).WithMany(p => p.transaction_product_details).HasConstraintName("transaction_product_details_transaction_id_fkey");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);
            entity.Property(e => e.created_at).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<user1>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.ToTable("users", "auth", tb => tb.HasComment("Auth: Stores user login data within a secure schema."));

            entity.HasIndex(e => e.confirmation_token, "confirmation_token_idx")
                .IsUnique()
                .HasFilter("((confirmation_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.email_change_token_current, "email_change_token_current_idx")
                .IsUnique()
                .HasFilter("((email_change_token_current)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.email_change_token_new, "email_change_token_new_idx")
                .IsUnique()
                .HasFilter("((email_change_token_new)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.reauthentication_token, "reauthentication_token_idx")
                .IsUnique()
                .HasFilter("((reauthentication_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.recovery_token, "recovery_token_idx")
                .IsUnique()
                .HasFilter("((recovery_token)::text !~ '^[0-9 ]*$'::text)");

            entity.HasIndex(e => e.email, "users_email_partial_key")
                .IsUnique()
                .HasFilter("(is_sso_user = false)");

            entity.Property(e => e.id).ValueGeneratedNever();
            entity.Property(e => e.confirmed_at).HasComputedColumnSql("LEAST(email_confirmed_at, phone_confirmed_at)", true);
            entity.Property(e => e.email_change_confirm_status).HasDefaultValue((short)0);
            entity.Property(e => e.email_change_token_current).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.is_anonymous).HasDefaultValue(false);
            entity.Property(e => e.is_sso_user)
                .HasDefaultValue(false)
                .HasComment("Auth: Set this column to true when the account comes from SSO. These accounts can have duplicate emails.");
            entity.Property(e => e.phone).HasDefaultValueSql("NULL::character varying");
            entity.Property(e => e.phone_change).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.phone_change_token).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.reauthentication_token).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<user_farm_role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("user_farm_role_pkey");

            entity.HasOne(d => d.farm).WithMany(p => p.user_farm_roles).HasConstraintName("user_farm_role_farm_id_fkey");

            entity.HasOne(d => d.role).WithMany(p => p.user_farm_roles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_farm_role_role_id_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.user_farm_roles).HasConstraintName("user_farm_role_user_id_fkey");
        });

        modelBuilder.Entity<v_low_stock_alert>(entity =>
        {
            entity.ToView("v_low_stock_alerts");
        });

        modelBuilder.Entity<vector_index>(entity =>
        {
            entity.HasKey(e => e.id).HasName("vector_indexes_pkey");

            entity.HasIndex(e => new { e.name, e.bucket_id }, "vector_indexes_name_bucket_id_idx")
                .IsUnique()
                .UseCollation(new[] { "C", null });

            entity.Property(e => e.id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at).HasDefaultValueSql("now()");
            entity.Property(e => e.name).UseCollation("C");
            entity.Property(e => e.updated_at).HasDefaultValueSql("now()");

            entity.HasOne(d => d.bucket).WithMany(p => p.vector_indices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vector_indexes_bucket_id_fkey");
        });

        modelBuilder.Entity<weighing>(entity =>
        {
            entity.HasKey(e => e.id).HasName("weighings_pkey");

            entity.Property(e => e.weighing_date).HasDefaultValueSql("CURRENT_DATE");

            entity.HasOne(d => d.animal).WithMany(p => p.weighings).HasConstraintName("weighings_animal_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.weighings).HasConstraintName("weighings_farm_id_fkey");

            entity.HasOne(d => d.registered_byNavigation).WithMany(p => p.weighings).HasConstraintName("weighings_registered_by_fkey");
        });

        modelBuilder.Entity<withdrawal_period>(entity =>
        {
            entity.HasKey(e => e.id).HasName("withdrawal_periods_pkey");

            entity.Property(e => e.active).HasDefaultValue(true);

            entity.HasOne(d => d.animal).WithMany(p => p.withdrawal_periods).HasConstraintName("withdrawal_periods_animal_id_fkey");

            entity.HasOne(d => d.farm).WithMany(p => p.withdrawal_periods).HasConstraintName("withdrawal_periods_farm_id_fkey");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
