using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddToptTwoFactorAuthUpdateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwoFactorToptSecret",
                schema: "Master",
                table: "User",
                type: "Char(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(160)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TwoFactorToptSecret",
                schema: "Master",
                table: "User",
                type: "VarChar(160)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Char(32)",
                oldNullable: true);
        }
    }
}
