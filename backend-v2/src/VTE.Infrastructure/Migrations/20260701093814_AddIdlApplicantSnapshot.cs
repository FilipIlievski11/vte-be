using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdlApplicantSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantAddress",
                table: "InternationalDrivingLicence",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantBirthPlace",
                table: "InternationalDrivingLicence",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantCitizenship",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantDateOfBirth",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantFirstName",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantIdCardDate",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantIdCardIssuer",
                table: "InternationalDrivingLicence",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantIdCardNumber",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantLastName",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantNationalLicenceDate",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantNationalLicenceIssuer",
                table: "InternationalDrivingLicence",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantParentName",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicantPassportDate",
                table: "InternationalDrivingLicence",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantPassportIssuer",
                table: "InternationalDrivingLicence",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantPassportNumber",
                table: "InternationalDrivingLicence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantAddress",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantBirthPlace",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantCitizenship",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantDateOfBirth",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantFirstName",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantIdCardDate",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantIdCardIssuer",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantIdCardNumber",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantLastName",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantNationalLicenceDate",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantNationalLicenceIssuer",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantParentName",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantPassportDate",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantPassportIssuer",
                table: "InternationalDrivingLicence");

            migrationBuilder.DropColumn(
                name: "ApplicantPassportNumber",
                table: "InternationalDrivingLicence");
        }
    }
}
