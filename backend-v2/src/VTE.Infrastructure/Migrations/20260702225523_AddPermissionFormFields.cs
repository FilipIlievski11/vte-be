using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionFormFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoorCount",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineIdMethod",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForPublicTransport",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasHook",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasWinch",
                table: "Vehicle",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "LyingSeats",
                table: "Vehicle",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiseTechSpec",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PowerPerCc",
                table: "Vehicle",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropulsionAxleCount",
                table: "Vehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthCityId",
                table: "Client",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Employer",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationsAllowed",
                table: "Client",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoorCount",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "EngineIdMethod",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "ForPublicTransport",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "HasHook",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "HasWinch",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "LyingSeats",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "NoiseTechSpec",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PowerPerCc",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PropulsionAxleCount",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "BirthCityId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Employer",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "NotificationsAllowed",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Client");
        }
    }
}
