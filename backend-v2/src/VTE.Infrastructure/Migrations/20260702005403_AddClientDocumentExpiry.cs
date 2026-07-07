using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientDocumentExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantNationalLicenceExpiry",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "ClientPersonalData",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantNationalLicenceExpiry",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "ClientPersonalData");
        }
    }
}
