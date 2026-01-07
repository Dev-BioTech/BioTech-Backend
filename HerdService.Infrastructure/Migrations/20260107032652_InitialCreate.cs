using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HerdService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    sex = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    min_age_months = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    max_age_months = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "batches",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_breeds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "movement_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    affects_inventory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    inventory_sign = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movement_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paddocks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    area_hectares = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    gauged_capacity = table.Column<int>(type: "integer", nullable: true),
                    grass_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    current_status = table.Column<string>(type: "text", nullable: false, defaultValue: "AVAILABLE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paddocks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "animals",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    visual_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    electronic_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    breed_id = table.Column<int>(type: "integer", nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    batch_id = table.Column<int>(type: "integer", nullable: true),
                    paddock_id = table.Column<int>(type: "integer", nullable: true),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    sex = table.Column<string>(type: "character(1)", fixedLength: true, maxLength: 1, nullable: false),
                    color = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    mother_id = table.Column<long>(type: "bigint", nullable: true),
                    father_id = table.Column<long>(type: "bigint", nullable: true),
                    external_mother = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    external_father = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    current_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "ACTIVE"),
                    purpose = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "MEAT"),
                    origin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    entry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    initial_cost = table.Column<decimal>(type: "numeric(12,2)", nullable: false, defaultValue: 0m),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animals", x => x.id);
                    table.ForeignKey(
                        name: "FK_animals_animal_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "animal_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_animals_animals_father_id",
                        column: x => x.father_id,
                        principalTable: "animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_animals_animals_mother_id",
                        column: x => x.mother_id,
                        principalTable: "animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_animals_batches_batch_id",
                        column: x => x.batch_id,
                        principalTable: "batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_animals_breeds_breed_id",
                        column: x => x.breed_id,
                        principalTable: "breeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_animals_paddocks_paddock_id",
                        column: x => x.paddock_id,
                        principalTable: "paddocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "animal_movements",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    farm_id = table.Column<int>(type: "integer", nullable: false),
                    animal_id = table.Column<long>(type: "bigint", nullable: false),
                    movement_type_id = table.Column<int>(type: "integer", nullable: false),
                    movement_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    previous_batch_id = table.Column<int>(type: "integer", nullable: true),
                    new_batch_id = table.Column<int>(type: "integer", nullable: true),
                    previous_paddock_id = table.Column<int>(type: "integer", nullable: true),
                    new_paddock_id = table.Column<int>(type: "integer", nullable: true),
                    third_party_id = table.Column<long>(type: "bigint", nullable: true),
                    transaction_value = table.Column<decimal>(type: "numeric(12,2)", nullable: true, defaultValue: 0m),
                    weight_at_movement = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    observations = table.Column<string>(type: "text", nullable: true),
                    registered_by = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedBy = table.Column<int>(type: "integer", nullable: true),
                    LastModifiedBy = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_movements", x => x.id);
                    table.ForeignKey(
                        name: "FK_animal_movements_animals_animal_id",
                        column: x => x.animal_id,
                        principalTable: "animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_animal_movements_batches_new_batch_id",
                        column: x => x.new_batch_id,
                        principalTable: "batches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_animal_movements_batches_previous_batch_id",
                        column: x => x.previous_batch_id,
                        principalTable: "batches",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_animal_movements_movement_types_movement_type_id",
                        column: x => x.movement_type_id,
                        principalTable: "movement_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_animal_movements_paddocks_new_paddock_id",
                        column: x => x.new_paddock_id,
                        principalTable: "paddocks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_animal_movements_paddocks_previous_paddock_id",
                        column: x => x.previous_paddock_id,
                        principalTable: "paddocks",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_categories_name",
                table: "animal_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_animal_id",
                table: "animal_movements",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_movement_type_id",
                table: "animal_movements",
                column: "movement_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_new_batch_id",
                table: "animal_movements",
                column: "new_batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_new_paddock_id",
                table: "animal_movements",
                column: "new_paddock_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_previous_batch_id",
                table: "animal_movements",
                column: "previous_batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_animal_movements_previous_paddock_id",
                table: "animal_movements",
                column: "previous_paddock_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_batch_id",
                table: "animals",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_breed_id",
                table: "animals",
                column: "breed_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_category_id",
                table: "animals",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_father_id",
                table: "animals",
                column: "father_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_mother_id",
                table: "animals",
                column: "mother_id");

            migrationBuilder.CreateIndex(
                name: "IX_animals_paddock_id",
                table: "animals",
                column: "paddock_id");

            migrationBuilder.CreateIndex(
                name: "uk_animal_code_farm",
                table: "animals",
                columns: new[] { "farm_id", "visual_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_batches_farm_id",
                table: "batches",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "IX_breeds_name",
                table: "breeds",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movement_types_name",
                table: "movement_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_paddock_code_farm",
                table: "paddocks",
                columns: new[] { "farm_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_movements");

            migrationBuilder.DropTable(
                name: "animals");

            migrationBuilder.DropTable(
                name: "movement_types");

            migrationBuilder.DropTable(
                name: "animal_categories");

            migrationBuilder.DropTable(
                name: "batches");

            migrationBuilder.DropTable(
                name: "breeds");

            migrationBuilder.DropTable(
                name: "paddocks");
        }
    }
}
