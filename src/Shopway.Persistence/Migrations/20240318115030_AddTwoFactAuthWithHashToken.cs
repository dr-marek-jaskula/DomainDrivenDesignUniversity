using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactAuthWithHashToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorToken",
                schema: "Master",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorTokenHash",
                schema: "Master",
                table: "User",
                type: "NChar(514)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorTokenHash",
                schema: "Master",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorToken",
                schema: "Master",
                table: "User",
                type: "VarChar(32)",
                nullable: true);
        }
    }
}
