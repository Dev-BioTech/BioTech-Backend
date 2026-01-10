using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FeedingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feeding_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    supply_date = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    diet_id = table.Column<int>(type: "integer", nullable: true),
                    batch_id = table.Column<int>(type: "integer", nullable: true),
                    animal_id = table.Column<long>(type: "bigint", nullable: true),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    total_quantity = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    animals_fed_count = table.Column<int>(type: "integer", nullable: false),
                    unit_cost_at_moment = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    calculated_total_cost = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    observations = table.Column<string>(type: "text", nullable: true),
                    IsCancelled = table.Column<bool>(type: "boolean", nullable: false),
                    registered_by = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    last_modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feeding_events", x => x.id);
                    table.CheckConstraint("check_feeding_destination", "(batch_id IS NOT NULL) <> (animal_id IS NOT NULL)");
                });

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_animal_id",
                table: "feeding_events",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_batch_id",
                table: "feeding_events",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_farm_id",
                table: "feeding_events",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_farm_supply_date",
                table: "feeding_events",
                columns: new[] { "farm_id", "supply_date" });

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_product_id",
                table: "feeding_events",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeding_events_supply_date",
                table: "feeding_events",
                column: "supply_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feeding_events");
        }
    }
}
