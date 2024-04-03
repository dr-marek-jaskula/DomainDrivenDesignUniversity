using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSessionForPaymentByClientSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "Shopway",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "Session",
                schema: "Shopway",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Session",
                schema: "Shopway",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                schema: "Shopway",
                table: "Payment",
                type: "VarChar(256)",
                nullable: true);
        }
    }
}
