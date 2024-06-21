using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingPermisison : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Master",
                table: "Permission",
                columns: new[] { "Name", "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { "INVALID_PERMISSION", null, null, null, "Other" });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[] { "INVALID_PERMISSION", "Administrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "INVALID_PERMISSION", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "INVALID_PERMISSION");
        }
    }
}
