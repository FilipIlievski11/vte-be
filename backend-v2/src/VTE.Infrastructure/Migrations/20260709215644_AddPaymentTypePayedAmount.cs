using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTypePayedAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Default TRUE (наплатен): повеќето типови се шалтерска наплата, а извештајот
            // ПРЕГЛЕД ЗА НАПЛАТА филтрира по овој флаг — со default false свежа база без
            // легаси-refresh би дала празни извештаи. Точните вредности (авансно/вирмански/
            // поништување = false) ги поставува PaymentType-refresh секцијата во
            // migrate/migrate-incremental.sql.
            migrationBuilder.AddColumn<bool>(
                name: "PayedAmount",
                table: "PaymentType",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayedAmount",
                table: "PaymentType");
        }
    }
}
