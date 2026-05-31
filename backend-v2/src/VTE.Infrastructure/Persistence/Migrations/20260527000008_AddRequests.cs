using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestAttachmentType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAttachmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestDocumentPrint",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TemplatePath = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDocumentPrint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestOwnershipProofType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestOwnershipProofType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestPaymentProofType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPaymentProofType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRequestTypeId = table.Column<byte>(type: "tinyint", nullable: true),
                    DocumentPrintId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TechnicalExamRequirement = table.Column<byte>(type: "tinyint", nullable: false),
                    PaymentRequired = table.Column<bool>(type: "bit", nullable: false),
                    IssuesNewRegistration = table.Column<bool>(type: "bit", nullable: false),
                    DeactivatesRelation = table.Column<bool>(type: "bit", nullable: false),
                    DeactivatesVehicle = table.Column<bool>(type: "bit", nullable: false),
                    TransfersOwnership = table.Column<bool>(type: "bit", nullable: false),
                    MutatesVehicleData = table.Column<bool>(type: "bit", nullable: false),
                    MutatesClientData = table.Column<bool>(type: "bit", nullable: false),
                    IsSufficient = table.Column<bool>(type: "bit", nullable: false),
                    PreviousRegistrationRequired = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestType_RequestDocumentPrint_DocumentPrintId",
                        column: x => x.DocumentPrintId,
                        principalTable: "RequestDocumentPrint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestType_RequestType_ParentRequestTypeId",
                        column: x => x.ParentRequestTypeId,
                        principalTable: "RequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    RequestTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ClientVehicleRelationId = table.Column<long>(type: "bigint", nullable: false),
                    NewClientVehicleRelationId = table.Column<long>(type: "bigint", nullable: true),
                    TechnicalExamReportId = table.Column<long>(type: "bigint", nullable: true),
                    PreviousRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModifiedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    EndedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    VehicleDataChanged = table.Column<bool>(type: "bit", nullable: false),
                    ClientDataChanged = table.Column<bool>(type: "bit", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_ClientVehicleRelation_ClientVehicleRelationId",
                        column: x => x.ClientVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_ClientVehicleRelation_NewClientVehicleRelationId",
                        column: x => x.NewClientVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Request_RequestType_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestAttachment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    AttachmentTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestAttachment_RequestAttachmentType_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "RequestAttachmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestAttachment_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestOwnershipProof",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    OwnershipProofTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestOwnershipProof", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestOwnershipProof_RequestOwnershipProofType_OwnershipProofTypeId",
                        column: x => x.OwnershipProofTypeId,
                        principalTable: "RequestOwnershipProofType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestOwnershipProof_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestPaymentProof",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentProofTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestPaymentProof", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestPaymentProof_RequestPaymentProofType_PaymentProofTypeId",
                        column: x => x.PaymentProofTypeId,
                        principalTable: "RequestPaymentProofType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestPaymentProof_Request_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_ClientVehicleRelationId",
                table: "Request",
                column: "ClientVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_NewClientVehicleRelationId",
                table: "Request",
                column: "NewClientVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_Open",
                table: "Request",
                columns: new[] { "CompanyId", "CreatedAt" },
                filter: "[Active] = 1 AND [EndedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Request_PreviousRegistrationId",
                table: "Request",
                column: "PreviousRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_RequestTypeId",
                table: "Request",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachment_AttachmentTypeId",
                table: "RequestAttachment",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAttachment_RequestId",
                table: "RequestAttachment",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestOwnershipProof_OwnershipProofTypeId",
                table: "RequestOwnershipProof",
                column: "OwnershipProofTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestOwnershipProof_RequestId",
                table: "RequestOwnershipProof",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPaymentProof_PaymentProofTypeId",
                table: "RequestPaymentProof",
                column: "PaymentProofTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPaymentProof_RequestId",
                table: "RequestPaymentProof",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestType_DocumentPrintId",
                table: "RequestType",
                column: "DocumentPrintId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestType_Name",
                table: "RequestType",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RequestType_ParentRequestTypeId",
                table: "RequestType",
                column: "ParentRequestTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestAttachment");

            migrationBuilder.DropTable(
                name: "RequestOwnershipProof");

            migrationBuilder.DropTable(
                name: "RequestPaymentProof");

            migrationBuilder.DropTable(
                name: "RequestAttachmentType");

            migrationBuilder.DropTable(
                name: "RequestOwnershipProofType");

            migrationBuilder.DropTable(
                name: "RequestPaymentProofType");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropTable(
                name: "RequestDocumentPrint");
        }
    }
}
