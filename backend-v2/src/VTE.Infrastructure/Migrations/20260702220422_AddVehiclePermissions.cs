using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehiclePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginPermissionId",
                table: "CustomerDebt",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VehiclePermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    ClientVehicleRelationId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizedClientId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerId = table.Column<byte>(type: "tinyint", nullable: false),
                    IssuingCityId = table.Column<int>(type: "int", nullable: false),
                    IssuerOrganizationId = table.Column<int>(type: "int", nullable: false),
                    PermissionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrafficLicenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TriptiqueNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidTillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    OwnerIdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AuthorizedName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AuthorizedEmbg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AuthorizedIdCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuthorizedPassportNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AuthorizedAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VehicleDisplay = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PlateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VehicleVin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VehicleEngineNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_City_IssuingCityId",
                        column: x => x.IssuingCityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_ClientVehicleRelation_ClientVehicleRelationId",
                        column: x => x.ClientVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_Client_AuthorizedClientId",
                        column: x => x.AuthorizedClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_DocumentIssuer_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "DocumentIssuer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehiclePermission_TechnicalExamOrganization_IssuerOrganizationId",
                        column: x => x.IssuerOrganizationId,
                        principalTable: "TechnicalExamOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_AuthorizedClientId",
                table: "VehiclePermission",
                column: "AuthorizedClientId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_AuthorizedClientId_ClientVehicleRelationId",
                table: "VehiclePermission",
                columns: new[] { "AuthorizedClientId", "ClientVehicleRelationId" },
                unique: true,
                filter: "[Active] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_ClientVehicleRelationId",
                table: "VehiclePermission",
                column: "ClientVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_CompanyId",
                table: "VehiclePermission",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_IssuerId",
                table: "VehiclePermission",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_IssuerOrganizationId",
                table: "VehiclePermission",
                column: "IssuerOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_VehiclePermission_IssuingCityId",
                table: "VehiclePermission",
                column: "IssuingCityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehiclePermission");

            migrationBuilder.DropColumn(
                name: "OriginPermissionId",
                table: "CustomerDebt");
        }
    }
}
