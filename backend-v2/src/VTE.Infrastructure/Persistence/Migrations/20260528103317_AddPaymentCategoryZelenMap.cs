using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentCategoryZelenMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ZelenMap",
                table: "VehiclePaymentCategory",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZelenMap",
                table: "VehiclePaymentCategory");
        }
    }
}
