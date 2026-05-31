using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleTowingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "MaxHitchLoadKg",
                table: "Vehicle",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxTrailerBrakedKg",
                table: "Vehicle",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxTrailerUnbrakedKg",
                table: "Vehicle",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHitchLoadKg",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "MaxTrailerBrakedKg",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "MaxTrailerUnbrakedKg",
                table: "Vehicle");
        }
    }
}
