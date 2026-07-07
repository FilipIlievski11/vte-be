using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionLegacyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehiclePermission_AuthorizedClientId_ClientVehicleRelationId",
                table: "VehiclePermission");

            migrationBuilder.AddColumn<long>(
                name: "LegacyId",
                table: "VehiclePermission",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_AuthorizedClientId_ClientVehicleRelationId",
                table: "VehiclePermission",
                columns: new[] { "AuthorizedClientId", "ClientVehicleRelationId" },
                unique: true,
                filter: "[Active] = 1 AND [LegacyId] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_LegacyId",
                table: "VehiclePermission",
                column: "LegacyId",
                unique: true,
                filter: "[LegacyId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehiclePermission_AuthorizedClientId_ClientVehicleRelationId",
                table: "VehiclePermission");

            migrationBuilder.DropIndex(
                name: "IX_VehiclePermission_LegacyId",
                table: "VehiclePermission");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "VehiclePermission");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_AuthorizedClientId_ClientVehicleRelationId",
                table: "VehiclePermission",
                columns: new[] { "AuthorizedClientId", "ClientVehicleRelationId" },
                unique: true,
                filter: "[Active] = 1");
        }
    }
}
