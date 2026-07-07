using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculationItemIsOwnAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOwnAccount",
                table: "CalculationItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CalculationItem_IsOwnAccount",
                table: "CalculationItem",
                column: "IsOwnAccount",
                filter: "[IsOwnAccount] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CalculationItem_IsOwnAccount",
                table: "CalculationItem");

            migrationBuilder.DropColumn(
                name: "IsOwnAccount",
                table: "CalculationItem");
        }
    }
}
