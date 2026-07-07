using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInternationalDrivingLicences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginInternationalDrivingLicenceId",
                table: "CustomerDebt",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthCityId",
                table: "Client",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DrivingLicenceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingLicenceCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternationalDrivingLicence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerOrganizationId = table.Column<int>(type: "int", nullable: false),
                    NumberOfLicence = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumberOfNationalLicence = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternationalDrivingLicence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternationalDrivingLicence_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalDrivingLicence_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalDrivingLicence_TechnicalExamOrganization_IssuerOrganizationId",
                        column: x => x.IssuerOrganizationId,
                        principalTable: "TechnicalExamOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InternationalDrivingLicenceCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternationalDrivingLicenceId = table.Column<long>(type: "bigint", nullable: false),
                    DrivingLicenceCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternationalDrivingLicenceCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternationalDrivingLicenceCategory_DrivingLicenceCategory_DrivingLicenceCategoryId",
                        column: x => x.DrivingLicenceCategoryId,
                        principalTable: "DrivingLicenceCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternationalDrivingLicenceCategory_InternationalDrivingLicence_InternationalDrivingLicenceId",
                        column: x => x.InternationalDrivingLicenceId,
                        principalTable: "InternationalDrivingLicence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_OriginInternationalDrivingLicenceId",
                table: "CustomerDebt",
                column: "OriginInternationalDrivingLicenceId",
                filter: "[OriginInternationalDrivingLicenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Client_BirthCityId",
                table: "Client",
                column: "BirthCityId");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_ClientId",
                table: "InternationalDrivingLicence",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_CompanyId",
                table: "InternationalDrivingLicence",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_IssuerOrganizationId",
                table: "InternationalDrivingLicence",
                column: "IssuerOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicence_NumberOfLicence",
                table: "InternationalDrivingLicence",
                column: "NumberOfLicence",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicenceCategory_DrivingLicenceCategoryId",
                table: "InternationalDrivingLicenceCategory",
                column: "DrivingLicenceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InternationalDrivingLicenceCategory_InternationalDrivingLicenceId_DrivingLicenceCategoryId",
                table: "InternationalDrivingLicenceCategory",
                columns: new[] { "InternationalDrivingLicenceId", "DrivingLicenceCategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Client_City_BirthCityId",
                table: "Client",
                column: "BirthCityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_City_BirthCityId",
                table: "Client");

            migrationBuilder.DropTable(
                name: "InternationalDrivingLicenceCategory");

            migrationBuilder.DropTable(
                name: "DrivingLicenceCategory");

            migrationBuilder.DropTable(
                name: "InternationalDrivingLicence");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDebt_OriginInternationalDrivingLicenceId",
                table: "CustomerDebt");

            migrationBuilder.DropIndex(
                name: "IX_Client_BirthCityId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "OriginInternationalDrivingLicenceId",
                table: "CustomerDebt");

            migrationBuilder.DropColumn(
                name: "BirthCityId",
                table: "Client");
        }
    }
}
