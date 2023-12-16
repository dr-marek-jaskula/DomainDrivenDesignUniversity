using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Permissions_Roles : Migration
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
                keyValues: new object[] { (byte)5, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)2 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)2, (byte)2 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)2, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)3, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)1, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)2, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)3, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { (byte)4, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)4);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)5);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyValue: (byte)4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Master",
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Read" },
                    { (byte)2, "Create" },
                    { (byte)3, "Update" },
                    { (byte)4, "Delete" },
                    { (byte)5, "CRUD_Review" }
                });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Customer" },
                    { (byte)2, "Employee" },
                    { (byte)3, "Manager" },
                    { (byte)4, "Administrator" }
                });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { (byte)1, (byte)1 },
                    { (byte)5, (byte)1 },
                    { (byte)1, (byte)2 },
                    { (byte)2, (byte)2 },
                    { (byte)1, (byte)3 },
                    { (byte)2, (byte)3 },
                    { (byte)3, (byte)3 },
                    { (byte)1, (byte)4 },
                    { (byte)2, (byte)4 },
                    { (byte)3, (byte)4 },
                    { (byte)4, (byte)4 }
                });
        }
    }
}
