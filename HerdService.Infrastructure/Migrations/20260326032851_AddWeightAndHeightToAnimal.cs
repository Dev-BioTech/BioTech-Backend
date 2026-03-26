using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerdService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightAndHeightToAnimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "height",
                table: "animals",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "weight",
                table: "animals",
                type: "numeric(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "height",
                table: "animals");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "animals");
        }
    }
}
