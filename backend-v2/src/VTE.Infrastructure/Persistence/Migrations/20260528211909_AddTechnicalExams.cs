using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicalExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechnicalExamDetailStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamDetailStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalExamOrganization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CommunityId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(510)", maxLength: 510, nullable: true),
                    Depositor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResponsibleOfficer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Secretary = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamOrganization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalExamType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ValidDays = table.Column<int>(type: "int", nullable: false),
                    PercentOfFullExam = table.Column<int>(type: "int", nullable: false),
                    IsInRegister = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalExamVehiclePart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamVehiclePart", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalExamReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    CustomerVehicleRelationId = table.Column<long>(type: "bigint", nullable: true),
                    TechnicalExamTypeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    RegNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MadeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTillDate = table.Column<DateOnly>(type: "date", nullable: false),
                    FirstControllerLegacyId = table.Column<int>(type: "int", nullable: true),
                    SecondControllerLegacyId = table.Column<int>(type: "int", nullable: true),
                    VehicleIsRight = table.Column<bool>(type: "bit", nullable: false),
                    ExplanationNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DriversWarning = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TechnicalChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Axis1Left = table.Column<double>(type: "float", nullable: true),
                    Axis1Right = table.Column<double>(type: "float", nullable: true),
                    Axis1Gj = table.Column<double>(type: "float", nullable: true),
                    Axis1LeftRightDiff = table.Column<double>(type: "float", nullable: true),
                    Axis1Coefficient = table.Column<double>(type: "float", nullable: true),
                    Axis2Left = table.Column<double>(type: "float", nullable: true),
                    Axis2Right = table.Column<double>(type: "float", nullable: true),
                    Axis2Gj = table.Column<double>(type: "float", nullable: true),
                    Axis2LeftRightDiff = table.Column<double>(type: "float", nullable: true),
                    Axis2Coefficient = table.Column<double>(type: "float", nullable: true),
                    Axis3Left = table.Column<double>(type: "float", nullable: true),
                    Axis3Right = table.Column<double>(type: "float", nullable: true),
                    Axis3Gj = table.Column<double>(type: "float", nullable: true),
                    Axis3LeftRightDiff = table.Column<double>(type: "float", nullable: true),
                    Axis3Coefficient = table.Column<double>(type: "float", nullable: true),
                    Axis4Left = table.Column<double>(type: "float", nullable: true),
                    Axis4Right = table.Column<double>(type: "float", nullable: true),
                    Axis4Gj = table.Column<double>(type: "float", nullable: true),
                    Axis4LeftRightDiff = table.Column<double>(type: "float", nullable: true),
                    Axis4Coefficient = table.Column<double>(type: "float", nullable: true),
                    AxisParkingLeft = table.Column<double>(type: "float", nullable: true),
                    AxisParkingRight = table.Column<double>(type: "float", nullable: true),
                    AxisParkingGj = table.Column<double>(type: "float", nullable: true),
                    AxisParkingLeftRightDiff = table.Column<double>(type: "float", nullable: true),
                    AxisParkingCoefficient = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    EffectOfWorkingBrakeEmpty = table.Column<double>(type: "float", nullable: true),
                    EffectOfWorkingBrakeFull = table.Column<double>(type: "float", nullable: true),
                    EffectOfSecondaryBrake = table.Column<double>(type: "float", nullable: true),
                    EffectOfParkingBrake = table.Column<double>(type: "float", nullable: true),
                    SpeedOfTurns = table.Column<double>(type: "float", nullable: true),
                    CO = table.Column<double>(type: "float", nullable: true),
                    EngineRpm = table.Column<double>(type: "float", nullable: true),
                    COPlusTurns = table.Column<double>(type: "float", nullable: true),
                    Lambda = table.Column<double>(type: "float", nullable: true),
                    Pinpoints = table.Column<double>(type: "float", nullable: true),
                    Noise = table.Column<double>(type: "float", nullable: true),
                    EngineOilTemp = table.Column<double>(type: "float", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReport_ClientVehicleRelation_CustomerVehicleRelationId",
                        column: x => x.CustomerVehicleRelationId,
                        principalTable: "ClientVehicleRelation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReport_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReport_TechnicalExamOrganization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "TechnicalExamOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReport_TechnicalExamType_TechnicalExamTypeId",
                        column: x => x.TechnicalExamTypeId,
                        principalTable: "TechnicalExamType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalExamReportDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicalExamReportId = table.Column<long>(type: "bigint", nullable: false),
                    VehiclePartId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Front = table.Column<bool>(type: "bit", nullable: false),
                    Back = table.Column<bool>(type: "bit", nullable: false),
                    OnLeft = table.Column<bool>(type: "bit", nullable: false),
                    OnRight = table.Column<bool>(type: "bit", nullable: false),
                    EnteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalExamReportDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReportDetail_TechnicalExamDetailStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TechnicalExamDetailStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReportDetail_TechnicalExamReport_TechnicalExamReportId",
                        column: x => x.TechnicalExamReportId,
                        principalTable: "TechnicalExamReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalExamReportDetail_TechnicalExamVehiclePart_VehiclePartId",
                        column: x => x.VehiclePartId,
                        principalTable: "TechnicalExamVehiclePart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamOrganization_CityId",
                table: "TechnicalExamOrganization",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReport_CompanyId_MadeDate",
                table: "TechnicalExamReport",
                columns: new[] { "CompanyId", "MadeDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReport_CustomerVehicleRelationId",
                table: "TechnicalExamReport",
                column: "CustomerVehicleRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReport_OrganizationId",
                table: "TechnicalExamReport",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReport_RegNumber",
                table: "TechnicalExamReport",
                column: "RegNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReport_TechnicalExamTypeId",
                table: "TechnicalExamReport",
                column: "TechnicalExamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReportDetail_StatusId",
                table: "TechnicalExamReportDetail",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReportDetail_TechnicalExamReportId",
                table: "TechnicalExamReportDetail",
                column: "TechnicalExamReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamReportDetail_VehiclePartId",
                table: "TechnicalExamReportDetail",
                column: "VehiclePartId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalExamVehiclePart_CategoryId",
                table: "TechnicalExamVehiclePart",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicalExamReportDetail");

            migrationBuilder.DropTable(
                name: "TechnicalExamDetailStatus");

            migrationBuilder.DropTable(
                name: "TechnicalExamReport");

            migrationBuilder.DropTable(
                name: "TechnicalExamVehiclePart");

            migrationBuilder.DropTable(
                name: "TechnicalExamOrganization");

            migrationBuilder.DropTable(
                name: "TechnicalExamType");
        }
    }
}
