using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdlDocExpiries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantIdCardExpiry",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantPassportExpiry",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantIdCardExpiry",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantPassportExpiry",
                table: "InternationalDrivingLicence");
        }
    }
}
