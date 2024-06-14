using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)2 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)4 });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "Name",
                value: "INVALID_PERMISSION");

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)5,
                column: "Name",
                value: "Review_Read");

            migrationBuilder.InsertData(
                schema: "Master",
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)6, "Product_Read" },
                    { (byte)7, "Product_Read_Customer" }
                });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { (byte)5, (byte)1 },
                    { (byte)5, (byte)2 },
                    { (byte)5, (byte)3 },
                    { (byte)5, (byte)4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)6);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)7);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)5, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)5, (byte)2 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)5, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)5, (byte)4 });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)1,
                column: "Name",
                value: "Review_Read");

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)5,
                column: "Name",
                value: "INVALID_PERMISSION");

            migrationBuilder.InsertData(
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { (byte)1, (byte)1 },
                    { (byte)1, (byte)2 },
                    { (byte)1, (byte)3 },
                    { (byte)1, (byte)4 }
                });
        }
    }
}
