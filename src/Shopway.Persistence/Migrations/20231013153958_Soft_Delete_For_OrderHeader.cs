using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Soft_Delete_For_OrderHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                schema: "Shopway",
                table: "OrderHeader",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SoftDeletedOn",
                schema: "Shopway",
                table: "OrderHeader",
                type: "DateTimeOffset(2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                schema: "Shopway",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "SoftDeletedOn",
                schema: "Shopway",
                table: "OrderHeader");
        }
    }
}
