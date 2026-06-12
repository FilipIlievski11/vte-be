using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientVehicleRelationType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false),
                    IsAuthorized = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomerOnly = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientVehicleRelationType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentIssuer",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentIssuer", x => x.Id);
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
                name: "PersonalDataType",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDataType", x => x.Id);
                });

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
                name: "VehicleBodyType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBodyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCategory",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleColor",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleColor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleEcoProgram",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleEcoProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleEngineType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleEngineType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleFuel",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleFuel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMaker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<short>(type: "smallint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Trademark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMaker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehiclePaymentCategory",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ZelenMap = table.Column<byte>(type: "tinyint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePaymentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Station",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Station_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Citizenship",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizenship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Citizenship_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CountryId = table.Column<short>(type: "smallint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlateNumberPrefix = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Community_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    VehiclePaymentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CommunityId = table.Column<int>(type: "int", nullable: true),
                    PriceCompanyId = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentCategoryGroupId = table.Column<int>(type: "int", nullable: true),
                    VehicleField = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    ParametarFrom = table.Column<double>(type: "float", nullable: true),
                    ParametarTo = table.Column<double>(type: "float", nullable: true),
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
                name: "VehicleModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductionStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductionEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleModel_VehicleMaker_MakerId",
                        column: x => x.MakerId,
                        principalTable: "VehicleMaker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    Vin = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    EngineNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Plate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CategoryId = table.Column<short>(type: "smallint", nullable: true),
                    BodyTypeId = table.Column<int>(type: "int", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: true),
                    PrimaryColorId = table.Column<short>(type: "smallint", nullable: true),
                    SecondaryColorId = table.Column<short>(type: "smallint", nullable: true),
                    MadeCountryId = table.Column<short>(type: "smallint", nullable: true),
                    FuelId = table.Column<byte>(type: "tinyint", nullable: true),
                    SecondFuelId = table.Column<byte>(type: "tinyint", nullable: true),
                    EngineTypeId = table.Column<int>(type: "int", nullable: true),
                    EcoProgramId = table.Column<byte>(type: "tinyint", nullable: true),
                    PaymentCategoryId = table.Column<byte>(type: "tinyint", nullable: true),
                    EnginePowerKw = table.Column<float>(type: "real", nullable: true),
                    EngineWorkingCapacityCc = table.Column<float>(type: "real", nullable: true),
                    MaxRpm = table.Column<int>(type: "int", nullable: true),
                    MaxSpeedKmh = table.Column<float>(type: "real", nullable: true),
                    HasLpg = table.Column<bool>(type: "bit", nullable: true),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LengthMm = table.Column<float>(type: "real", nullable: true),
                    WidthMm = table.Column<float>(type: "real", nullable: true),
                    HeightMm = table.Column<float>(type: "real", nullable: true),
                    EmptyWeightKg = table.Column<float>(type: "real", nullable: true),
                    MaxAllowedWeightKg = table.Column<float>(type: "real", nullable: true),
                    MaxLegalTotalMassKg = table.Column<float>(type: "real", nullable: true),
                    MaxConstructiveTotalMassKg = table.Column<float>(type: "real", nullable: true),
                    MaxLegalGroupMassKg = table.Column<float>(type: "real", nullable: true),
                    TrailerMassWithBrakesKg = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    TrailerMassWithoutBrakesKg = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    MaxTrailerBrakedKg = table.Column<float>(type: "real", nullable: true),
                    MaxTrailerUnbrakedKg = table.Column<float>(type: "real", nullable: true),
                    MaxHitchLoadKg = table.Column<float>(type: "real", nullable: true),
                    AxleCount = table.Column<int>(type: "int", nullable: true),
                    WheelCount = table.Column<int>(type: "int", nullable: true),
                    AxleLoad1Kg = table.Column<int>(type: "int", nullable: true),
                    AxleLoad2Kg = table.Column<int>(type: "int", nullable: true),
                    Seats = table.Column<short>(type: "smallint", nullable: true),
                    StandingSeats = table.Column<short>(type: "smallint", nullable: true),
                    Co2GKm = table.Column<float>(type: "real", nullable: true),
                    NoiseStaticDb = table.Column<float>(type: "real", nullable: true),
                    NoiseMovingDb = table.Column<float>(type: "real", nullable: true),
                    TypeText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ModelVariant = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ApprovalMark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicle_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_Country_MadeCountryId",
                        column: x => x.MadeCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleBodyType_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "VehicleBodyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "VehicleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleColor_PrimaryColorId",
                        column: x => x.PrimaryColorId,
                        principalTable: "VehicleColor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleColor_SecondaryColorId",
                        column: x => x.SecondaryColorId,
                        principalTable: "VehicleColor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleEcoProgram_EcoProgramId",
                        column: x => x.EcoProgramId,
                        principalTable: "VehicleEcoProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleEngineType_EngineTypeId",
                        column: x => x.EngineTypeId,
                        principalTable: "VehicleEngineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleFuel_FuelId",
                        column: x => x.FuelId,
                        principalTable: "VehicleFuel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleFuel_SecondFuelId",
                        column: x => x.SecondFuelId,
                        principalTable: "VehicleFuel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleModel_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehicleModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehiclePaymentCategory_PaymentCategoryId",
                        column: x => x.PaymentCategoryId,
                        principalTable: "VehiclePaymentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<byte>(type: "tinyint", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    CitizenshipId = table.Column<byte>(type: "tinyint", nullable: true),
                    Business = table.Column<bool>(type: "bit", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MB = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Citizenship_CitizenshipId",
                        column: x => x.CitizenshipId,
                        principalTable: "Citizenship",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleRegistration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    IssuerId = table.Column<byte>(type: "tinyint", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFirstRegistration = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleRegistration_DocumentIssuer_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "DocumentIssuer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleRegistration_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPersonalData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    PersonalDataTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    DocumentIssuerId = table.Column<byte>(type: "tinyint", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPersonalData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPersonalData_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientPersonalData_DocumentIssuer_DocumentIssuerId",
                        column: x => x.DocumentIssuerId,
                        principalTable: "DocumentIssuer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientPersonalData_PersonalDataType_PersonalDataTypeId",
                        column: x => x.PersonalDataTypeId,
                        principalTable: "PersonalDataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientVehicleRelation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true),
                    RelationTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EndNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientVehicleRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientVehicleRelation_ClientVehicleRelationType_RelationTypeId",
                        column: x => x.RelationTypeId,
                        principalTable: "ClientVehicleRelationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientVehicleRelation_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientVehicleRelation_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    LegacyReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Citizenship_CountryId",
                table: "Citizenship",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_City_CommunityId",
                table: "City",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CitizenshipId",
                table: "Client",
                column: "CitizenshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CityId",
                table: "Client",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyId",
                table: "Client",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPersonalData_ClientId",
                table: "ClientPersonalData",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPersonalData_DocumentIssuerId",
                table: "ClientPersonalData",
                column: "DocumentIssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPersonalData_PersonalDataTypeId",
                table: "ClientPersonalData",
                column: "PersonalDataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientVehicleRelation_ClientId",
                table: "ClientVehicleRelation",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientVehicleRelation_RelationTypeId",
                table: "ClientVehicleRelation",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientVehicleRelation_VehicleId",
                table: "ClientVehicleRelation",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Community_CountryId",
                table: "Community",
                column: "CountryId");

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
                name: "IX_PriceCatalog_Eval",
                table: "PriceCatalog",
                columns: new[] { "Trigger", "VehiclePaymentCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_Trigger",
                table: "PriceCatalog",
                column: "Trigger");

            migrationBuilder.CreateIndex(
                name: "IX_PriceCatalog_VatRateId",
                table: "PriceCatalog",
                column: "VatRateId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Station_CompanyId",
                table: "Station",
                column: "CompanyId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_BodyTypeId",
                table: "Vehicle",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CategoryId",
                table: "Vehicle",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_CompanyId",
                table: "Vehicle",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_EcoProgramId",
                table: "Vehicle",
                column: "EcoProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_EngineTypeId",
                table: "Vehicle",
                column: "EngineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_FuelId",
                table: "Vehicle",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_MadeCountryId",
                table: "Vehicle",
                column: "MadeCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ModelId",
                table: "Vehicle",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_PaymentCategoryId",
                table: "Vehicle",
                column: "PaymentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Plate",
                table: "Vehicle",
                column: "Plate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_PrimaryColorId",
                table: "Vehicle",
                column: "PrimaryColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_SecondaryColorId",
                table: "Vehicle",
                column: "SecondaryColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_SecondFuelId",
                table: "Vehicle",
                column: "SecondFuelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Vin",
                table: "Vehicle",
                column: "Vin");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaker_CountryId",
                table: "VehicleMaker",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModel_MakerId",
                table: "VehicleModel",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRegistration_IssuerId",
                table: "VehicleRegistration",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRegistration_PlateNumber",
                table: "VehicleRegistration",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleRegistration_VehicleId",
                table: "VehicleRegistration",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClientPersonalData");

            migrationBuilder.DropTable(
                name: "CustomerDebt");

            migrationBuilder.DropTable(
                name: "InstallmentSchedule");

            migrationBuilder.DropTable(
                name: "RequestAttachment");

            migrationBuilder.DropTable(
                name: "RequestOwnershipProof");

            migrationBuilder.DropTable(
                name: "RequestPaymentProof");

            migrationBuilder.DropTable(
                name: "Station");

            migrationBuilder.DropTable(
                name: "TechnicalExamReportDetail");

            migrationBuilder.DropTable(
                name: "VehicleRegistration");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PersonalDataType");

            migrationBuilder.DropTable(
                name: "PaymentDocumentLine");

            migrationBuilder.DropTable(
                name: "RequestAttachmentType");

            migrationBuilder.DropTable(
                name: "RequestOwnershipProofType");

            migrationBuilder.DropTable(
                name: "RequestPaymentProofType");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "TechnicalExamDetailStatus");

            migrationBuilder.DropTable(
                name: "TechnicalExamReport");

            migrationBuilder.DropTable(
                name: "TechnicalExamVehiclePart");

            migrationBuilder.DropTable(
                name: "DocumentIssuer");

            migrationBuilder.DropTable(
                name: "PaymentDocument");

            migrationBuilder.DropTable(
                name: "PriceCatalog");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropTable(
                name: "TechnicalExamOrganization");

            migrationBuilder.DropTable(
                name: "TechnicalExamType");

            migrationBuilder.DropTable(
                name: "ClientVehicleRelation");

            migrationBuilder.DropTable(
                name: "InstallmentAgreement");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "VatRate");

            migrationBuilder.DropTable(
                name: "RequestDocumentPrint");

            migrationBuilder.DropTable(
                name: "ClientVehicleRelationType");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Citizenship");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "VehicleBodyType");

            migrationBuilder.DropTable(
                name: "VehicleCategory");

            migrationBuilder.DropTable(
                name: "VehicleColor");

            migrationBuilder.DropTable(
                name: "VehicleEcoProgram");

            migrationBuilder.DropTable(
                name: "VehicleEngineType");

            migrationBuilder.DropTable(
                name: "VehicleFuel");

            migrationBuilder.DropTable(
                name: "VehicleModel");

            migrationBuilder.DropTable(
                name: "VehiclePaymentCategory");

            migrationBuilder.DropTable(
                name: "Community");

            migrationBuilder.DropTable(
                name: "VehicleMaker");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
