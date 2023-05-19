using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Payment_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                schema: "Shopway",
                table: "Payment",
                newName: "Price");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                schema: "Shopway",
                table: "Payment",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,2)",
                oldPrecision: 3,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                schema: "Shopway",
                table: "OrderHeader",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                schema: "Shopway",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Shopway",
                table: "Payment",
                newName: "Discount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                schema: "Shopway",
                table: "Payment",
                type: "decimal(3,2)",
                precision: 3,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }
    }
}
