using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTwoFactAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwoFactorToken",
                schema: "Master",
                table: "User",
                type: "VarChar(32)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwoFactorTokenCreatedOn",
                schema: "Master",
                table: "User",
                type: "DateTimeOffset(2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorToken",
                schema: "Master",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TwoFactorTokenCreatedOn",
                schema: "Master",
                table: "User");
        }
    }
}
