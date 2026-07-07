using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RevertClientProfileAndBirthCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_City_BirthCityId",
                table: "Client");

            migrationBuilder.DropTable(
                name: "BusinessType");

            migrationBuilder.DropIndex(
                name: "IX_Client_BirthCityId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_BusinessTypeId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "AddressNumber",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BirthCityId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "BusinessTypeId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CanSendNotifications",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Employer",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "Client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressNumber",
                table: "Client",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthCityId",
                table: "Client",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessTypeId",
                table: "Client",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanSendNotifications",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Employer",
                table: "Client",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Client",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Client",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "Client",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_BirthCityId",
                table: "Client",
                column: "BirthCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_BusinessTypeId",
                table: "Client",
                column: "BusinessTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_City_BirthCityId",
                table: "Client",
                column: "BirthCityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
