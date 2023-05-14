using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Shopway",
                table: "Payment",
                type: "VarChar(11)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Shopway",
                table: "Payment",
                type: "VarChar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(11)");
        }
    }
}
