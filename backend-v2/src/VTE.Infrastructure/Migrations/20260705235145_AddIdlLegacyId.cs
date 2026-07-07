using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdlLegacyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InternationalDrivingLicence_NumberOfLicence",
                table: "InternationalDrivingLicence");

            migrationBuilder.AddColumn<long>(
                name: "LegacyId",
                table: "InternationalDrivingLicence",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_LegacyId",
                table: "InternationalDrivingLicence",
                column: "LegacyId",
                unique: true,
                filter: "[LegacyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_NumberOfLicence",
                table: "InternationalDrivingLicence",
                column: "NumberOfLicence",
                unique: true,
                filter: "[LegacyId] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InternationalDrivingLicence_LegacyId",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropIndex(
                name: "IX_InternationalDrivingLicence_NumberOfLicence",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "LegacyId",
                table: "InternationalDrivingLicence");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_NumberOfLicence",
                table: "InternationalDrivingLicence",
                column: "NumberOfLicence",
                unique: true);
        }
    }
}
