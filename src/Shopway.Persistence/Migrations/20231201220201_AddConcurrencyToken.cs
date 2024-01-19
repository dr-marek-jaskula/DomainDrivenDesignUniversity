using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Master",
                table: "User",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Review",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Product",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Payment",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "OrderLine",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "OrderHeader",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Master",
                table: "Customer",
                type: "DateTimeOffset(7)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Master",
                table: "User",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Review",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Product",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "Payment",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "OrderLine",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Shopway",
                table: "OrderHeader",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedOn",
                schema: "Master",
                table: "Customer",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(7)",
                oldNullable: true);
        }
    }
}
