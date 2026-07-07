using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingCategoryLookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Form = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCategoryGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CalculationItemId = table.Column<int>(type: "int", nullable: true),
                    CompanyScope = table.Column<int>(type: "int", nullable: false),
                    VisibleOrder = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCategoryGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCategoryGroup_CalculationItem_CalculationItemId",
                        column: x => x.CalculationItemId,
                        principalTable: "CalculationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCategoryGroup_CalculationItemId",
                table: "PaymentCategoryGroup",
                column: "CalculationItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentCategoryGroup");

            migrationBuilder.DropTable(
                name: "CalculationItem");
        }
    }
}
