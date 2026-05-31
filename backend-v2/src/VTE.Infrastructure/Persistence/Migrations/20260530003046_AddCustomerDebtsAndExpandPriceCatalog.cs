using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerDebtsAndExpandPriceCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "PriceCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ParametarFrom",
                table: "PriceCatalog",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ParametarTo",
                table: "PriceCatalog",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentCategoryGroupId",
                table: "PriceCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleField",
                table: "PriceCatalog",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehiclePaymentCategoryId",
                table: "PriceCatalog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerDebt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    CustomerVehicleRelationId = table.Column<long>(type: "bigint", nullable: false),
                    PriceCatalogId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    VatPercent = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Origin = table.Column<byte>(type: "tinyint", nullable: false),
                    OriginRequestId = table.Column<long>(type: "bigint", nullable: true),
                    OriginTechnicalExamId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    SettledByLineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerDebt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerDebt_ClientVehicleRelation_CustomerVehicleRelationId",
                        column: x => x.CustomerVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDebt_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDebt_PaymentDocumentLine_SettledByLineId",
                        column: x => x.SettledByLineId,
                        principalTable: "PaymentDocumentLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerDebt_PriceCatalog_PriceCatalogId",
                        column: x => x.PriceCatalogId,
                        principalTable: "PriceCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_Eval",
                table: "PriceCatalog",
                columns: new[] { "Trigger", "VehiclePaymentCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_CompanyId",
                table: "CustomerDebt",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_CustomerVehicleRelationId",
                table: "CustomerDebt",
                column: "CustomerVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_Open",
                table: "CustomerDebt",
                columns: new[] { "CustomerVehicleRelationId", "Paid" },
                filter: "[Active] = 1 AND [Paid] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_OriginRequestId",
                table: "CustomerDebt",
                column: "OriginRequestId",
                filter: "[OriginRequestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_OriginTechnicalExamId",
                table: "CustomerDebt",
                column: "OriginTechnicalExamId",
                filter: "[OriginTechnicalExamId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_PriceCatalogId",
                table: "CustomerDebt",
                column: "PriceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDebt_SettledByLineId",
                table: "CustomerDebt",
                column: "SettledByLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerDebt");

            migrationBuilder.DropIndex(
                name: "IX_PriceCatalog_Eval",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "ParametarFrom",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "ParametarTo",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "PaymentCategoryGroupId",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "VehicleField",
                table: "PriceCatalog");

            migrationBuilder.DropColumn(
                name: "VehiclePaymentCategoryId",
                table: "PriceCatalog");
        }
    }
}
