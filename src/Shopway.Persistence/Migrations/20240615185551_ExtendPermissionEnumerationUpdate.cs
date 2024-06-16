using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendPermissionEnumerationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)6,
                column: "RelatedEntity",
                value: "Product");

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)7,
                column: "RelatedEntity",
                value: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)6,
                column: "RelatedEntity",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)7,
                column: "RelatedEntity",
                value: null);
        }
    }
}
