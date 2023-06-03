using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Unique_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Username_Email",
                schema: "Master",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Review_ProductId",
                schema: "Shopway",
                table: "Review");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "Master",
                table: "Customer",
                type: "VarChar(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(7)");

            migrationBuilder.CreateIndex(
                name: "UX_Username_Email",
                schema: "Master",
                table: "User",
                column: "Username",
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Email" });

            migrationBuilder.CreateIndex(
                name: "UX_Review_ProductId_Title",
                schema: "Shopway",
                table: "Review",
                columns: new[] { "ProductId", "Title" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Product_ProductName_Revision",
                schema: "Shopway",
                table: "Product",
                columns: new[] { "ProductName", "Revision" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Username_Email",
                schema: "Master",
                table: "User");

            migrationBuilder.DropIndex(
                name: "UX_Review_ProductId_Title",
                schema: "Shopway",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "UX_Product_ProductName_Revision",
                schema: "Shopway",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                schema: "Master",
                table: "Customer",
                type: "VarChar(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(6)");

            migrationBuilder.CreateIndex(
                name: "UX_Username_Email",
                schema: "Master",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProductId",
                schema: "Shopway",
                table: "Review",
                column: "ProductId");
        }
    }
}
