using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReproductionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reproduction_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    event_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    animal_id = table.Column<long>(type: "bigint", nullable: false),
                    event_type = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    male_animal_id = table.Column<int>(type: "integer", nullable: true),
                    is_successful = table.Column<bool>(type: "boolean", nullable: true),
                    expected_birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    offspring_count = table.Column<int>(type: "integer", nullable: true),
                    is_cancelled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    last_modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reproduction_events", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reproduction_events_animal_id",
                table: "reproduction_events",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_reproduction_events_event_type",
                table: "reproduction_events",
                column: "event_type");

            migrationBuilder.CreateIndex(
                name: "IX_reproduction_events_farm_id_event_date",
                table: "reproduction_events",
                columns: new[] { "farm_id", "event_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reproduction_events");
        }
    }
}
