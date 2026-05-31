using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstallmentAgreement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalInstallments = table.Column<int>(type: "int", nullable: false),
                    GuarantorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GuarantorAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    GuarantorEmbg = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallmentAgreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallmentAgreement_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsCash = table.Column<bool>(type: "bit", nullable: false),
                    IsCard = table.Column<bool>(type: "bit", nullable: false),
                    IsInstallment = table.Column<bool>(type: "bit", nullable: false),
                    PrintsReceipt = table.Column<bool>(type: "bit", nullable: false),
                    PrintsInvoice = table.Column<bool>(type: "bit", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VatRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Percent = table.Column<double>(type: "float", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatRate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomerVehicleRelationId = table.Column<long>(type: "bigint", nullable: false),
                    OperatorLegacyId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    Stornoed = table.Column<bool>(type: "bit", nullable: false),
                    StornoReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AgreementId = table.Column<long>(type: "bigint", nullable: true),
                    InvoicedToCompanyId = table.Column<int>(type: "int", nullable: true),
                    FiscalPrintedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegacyId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ModifiedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDocument_ClientVehicleRelation_CustomerVehicleRelationId",
                        column: x => x.CustomerVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentDocument_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentDocument_InstallmentAgreement_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "InstallmentAgreement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentDocument_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceCatalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    VatRateId = table.Column<int>(type: "int", nullable: false),
                    Trigger = table.Column<byte>(type: "tinyint", nullable: false),
                    VehicleCategoryFilter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentForm = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceCatalog_VatRate_VatRateId",
                        column: x => x.VatRateId,
                        principalTable: "VatRate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstallmentSchedule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    PaymentDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    OperatorLegacyId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallmentSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstallmentSchedule_PaymentDocument_PaymentDocumentId",
                        column: x => x.PaymentDocumentId,
                        principalTable: "PaymentDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDocumentLine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    PaymentDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    PriceCatalogId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    VatPercent = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PrePaid = table.Column<bool>(type: "bit", nullable: false),
                    PrePaidNote = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CustomerDebtId = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDocumentLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentDocumentLine_PaymentDocument_PaymentDocumentId",
                        column: x => x.PaymentDocumentId,
                        principalTable: "PaymentDocument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentDocumentLine_PriceCatalog_PriceCatalogId",
                        column: x => x.PriceCatalogId,
                        principalTable: "PriceCatalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentAgreement_CompanyId",
                table: "InstallmentAgreement",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentAgreement_Number",
                table: "InstallmentAgreement",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentSchedule_PaymentDocumentId",
                table: "InstallmentSchedule",
                column: "PaymentDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstallmentSchedule_PaymentDocumentId_SequenceNo",
                table: "InstallmentSchedule",
                columns: new[] { "PaymentDocumentId", "SequenceNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_AgreementId",
                table: "PaymentDocument",
                column: "AgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_CompanyId_IssueDate",
                table: "PaymentDocument",
                columns: new[] { "CompanyId", "IssueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_CustomerVehicleRelationId",
                table: "PaymentDocument",
                column: "CustomerVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_DocumentNumber",
                table: "PaymentDocument",
                column: "DocumentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_LegacyId",
                table: "PaymentDocument",
                column: "LegacyId",
                filter: "[LegacyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocument_PaymentTypeId",
                table: "PaymentDocument",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocumentLine_CustomerDebtId",
                table: "PaymentDocumentLine",
                column: "CustomerDebtId",
                filter: "[CustomerDebtId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocumentLine_PaymentDocumentId",
                table: "PaymentDocumentLine",
                column: "PaymentDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDocumentLine_PriceCatalogId",
                table: "PaymentDocumentLine",
                column: "PriceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_Trigger",
                table: "PriceCatalog",
                column: "Trigger");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_VatRateId",
                table: "PriceCatalog",
                column: "VatRateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstallmentSchedule");

            migrationBuilder.DropTable(
                name: "PaymentDocumentLine");

            migrationBuilder.DropTable(
                name: "PaymentDocument");

            migrationBuilder.DropTable(
                name: "PriceCatalog");

            migrationBuilder.DropTable(
                name: "InstallmentAgreement");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "VatRate");
        }
    }
}
