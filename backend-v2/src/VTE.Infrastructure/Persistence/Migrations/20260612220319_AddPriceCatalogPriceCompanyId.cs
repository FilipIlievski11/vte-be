using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VTE.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceCatalogPriceCompanyId : Migration
    {
        /// <inheritdoc />
        /// <remarks>Catch-up migration: the column was originally added via raw SQL
        /// (migrate/add-pricecatalog-company-and-backfill.sql), so existing databases
        /// already have it. Guarded so it works on both fresh and patched databases.</remarks>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF COL_LENGTH(N'dbo.PriceCatalog', N'PriceCompanyId') IS NULL
                      ALTER TABLE [dbo].[PriceCatalog] ADD [PriceCompanyId] tinyint NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF COL_LENGTH(N'dbo.PriceCatalog', N'PriceCompanyId') IS NOT NULL
                      ALTER TABLE [dbo].[PriceCatalog] DROP COLUMN [PriceCompanyId];");
        }
    }
}
