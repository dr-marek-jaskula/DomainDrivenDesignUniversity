using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalIdentityProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalIdentityProvider",
                schema: "Master",
                table: "User",
                type: "VarChar(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalIdentityProvider",
                schema: "Master",
                table: "User");
        }
    }
}
