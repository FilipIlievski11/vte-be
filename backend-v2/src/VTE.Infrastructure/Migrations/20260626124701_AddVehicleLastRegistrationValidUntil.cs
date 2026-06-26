using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleLastRegistrationValidUntil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRegistrationValidUntil",
                table: "Vehicle",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRegistrationValidUntil",
                table: "Vehicle");
        }
    }
}
