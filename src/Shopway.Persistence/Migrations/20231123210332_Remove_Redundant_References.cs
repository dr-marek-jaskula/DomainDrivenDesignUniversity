using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Redundant_References : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderHeaderId",
                schema: "Shopway",
                table: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderHeaderId",
                schema: "Shopway",
                table: "Payment",
                type: "Char(26)",
                nullable: false,
                defaultValue: "");
        }
    }
}
