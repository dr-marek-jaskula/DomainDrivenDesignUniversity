using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderLineWithProductSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLine_Product_ProductId",
                schema: "Shopway",
                table: "OrderLine");

            migrationBuilder.DropIndex(
                name: "IX_OrderLine_ProductId",
                schema: "Shopway",
                table: "OrderLine");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Shopway",
                table: "OrderLine");

            migrationBuilder.AddColumn<string>(
                name: "ProductSummary",
                schema: "Shopway",
                table: "OrderLine",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSummary",
                schema: "Shopway",
                table: "OrderLine");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                schema: "Shopway",
                table: "OrderLine",
                type: "Char(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_ProductId",
                schema: "Shopway",
                table: "OrderLine",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLine_Product_ProductId",
                schema: "Shopway",
                table: "OrderLine",
                column: "ProductId",
                principalSchema: "Shopway",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
