using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicles : Migration
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
                    LengthMm = table.Column<float>(type: "real", nullable: true),
                    WidthMm = table.Column<float>(type: "real", nullable: true),
                    HeightMm = table.Column<float>(type: "real", nullable: true),
                    EmptyWeightKg = table.Column<float>(type: "real", nullable: true),
                    MaxAllowedWeightKg = table.Column<float>(type: "real", nullable: true),
                    MaxLegalTotalMassKg = table.Column<float>(type: "real", nullable: true),
                    MaxConstructiveTotalMassKg = table.Column<float>(type: "real", nullable: true),
                    TrailerMassWithBrakesKg = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    TrailerMassWithoutBrakesKg = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
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
                name: "ClientVehicleRelation");

            migrationBuilder.DropTable(
                name: "VehicleRegistration");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ClientVehicleRelationType");

            migrationBuilder.DropTable(
                name: "Vehicle");

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
                name: "VehicleMaker");
        }
    }
}
