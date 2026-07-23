using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceCatalogAgeRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgeFrom",
                table: "PriceCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgeTo",
                table: "PriceCatalog",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeFrom",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "AgeTo",
                table: "PriceCatalog");
        }
    }
}
