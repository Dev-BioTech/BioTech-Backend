using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommercialService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commercial_transactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    third_party_id = table.Column<long>(type: "bigint", nullable: true),
                    transaction_type = table.Column<string>(type: "text", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    invoice_number = table.Column<string>(type: "text", nullable: true),
                    subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    taxes = table.Column<decimal>(type: "numeric", nullable: false),
                    discounts = table.Column<decimal>(type: "numeric", nullable: false),
                    net_total = table.Column<decimal>(type: "numeric", nullable: true),
                    payment_status = table.Column<string>(type: "text", nullable: false),
                    observations = table.Column<string>(type: "text", nullable: true),
                    registered_by = table.Column<int>(type: "integer", nullable: true),
                    registered_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commercial_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "third_parties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    identity_document = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    is_supplier = table.Column<bool>(type: "boolean", nullable: false),
                    is_customer = table.Column<bool>(type: "boolean", nullable: false),
                    is_employee = table.Column<bool>(type: "boolean", nullable: false),
                    is_veterinarian = table.Column<bool>(type: "boolean", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_third_parties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_animal_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    animal_id = table.Column<long>(type: "bigint", nullable: false),
                    price_per_kilo = table.Column<decimal>(type: "numeric", nullable: true),
                    weight_at_negotiation = table.Column<decimal>(type: "numeric", nullable: true),
                    base_head_price = table.Column<decimal>(type: "numeric", nullable: false),
                    commission_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    transport_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    final_line_value = table.Column<decimal>(type: "numeric", nullable: true),
                    animal_movement_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_animal_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_animal_details_commercial_transactions_transact~",
                        column: x => x.transaction_id,
                        principalTable: "commercial_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_product_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                    line_subtotal = table.Column<decimal>(type: "numeric", nullable: true),
                    inventory_movement_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_product_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_product_details_commercial_transactions_transac~",
                        column: x => x.transaction_id,
                        principalTable: "commercial_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_third_parties_farm_id_identity_document",
                table: "third_parties",
                columns: new[] { "farm_id", "identity_document" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_animal_details_transaction_id",
                table: "transaction_animal_details",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_product_details_transaction_id",
                table: "transaction_product_details",
                column: "transaction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "third_parties");

            migrationBuilder.DropTable(
                name: "transaction_animal_details");

            migrationBuilder.DropTable(
                name: "transaction_product_details");

            migrationBuilder.DropTable(
                name: "commercial_transactions");
        }
    }
}
