using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HealthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diseases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diseases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "health_events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    animal_id = table.Column<long>(type: "bigint", nullable: true),
                    batch_id = table.Column<int>(type: "integer", nullable: true),
                    event_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    event_date = table.Column<DateOnly>(type: "date", nullable: false),
                    disease = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    treatment = table.Column<string>(type: "text", nullable: true),
                    medication = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    dosage = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    dosage_unit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    veterinarian_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    cost = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    next_follow_up_date = table.Column<DateOnly>(type: "date", nullable: true),
                    requires_follow_up = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    follow_up_notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    last_modified_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_health_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "withdrawal_periods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FarmId = table.Column<int>(type: "integer", nullable: false),
                    AnimalId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WithdrawalDays = table.Column<int>(type: "integer", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProductType = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_withdrawal_periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "health_event_details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HealthEventId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    DosePerAnimal = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalQuantityDeducted = table.Column<decimal>(type: "numeric", nullable: false),
                    unit_cost_at_moment = table.Column<decimal>(type: "numeric", nullable: false),
                    calculated_total_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    AdministrationRoute = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_health_event_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_health_event_details_health_events_HealthEventId",
                        column: x => x.HealthEventId,
                        principalTable: "health_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_health_event_details_HealthEventId",
                table: "health_event_details",
                column: "HealthEventId");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_animal_id",
                table: "health_events",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_batch_id",
                table: "health_events",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_event_date",
                table: "health_events",
                column: "event_date");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_event_type",
                table: "health_events",
                column: "event_type");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_farm_id",
                table: "health_events",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "IX_health_events_farm_id_event_date",
                table: "health_events",
                columns: new[] { "farm_id", "event_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "diseases");

            migrationBuilder.DropTable(
                name: "health_event_details");

            migrationBuilder.DropTable(
                name: "withdrawal_periods");

            migrationBuilder.DropTable(
                name: "health_events");
        }
    }
}
