using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RoleId",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUser",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                schema: "Master",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_Name",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)6);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)7);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)2, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)3, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)5, (byte)1 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)5, (byte)2 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)2, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)3, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)5, (byte)3 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)2, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)3, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)4, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyColumnTypes: new[] { "TinyInt", "TinyInt" },
                keyValues: new object[] { (byte)5, (byte)4 });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)4);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)5);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)3);

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Id",
                keyColumnType: "TinyInt",
                keyValue: (byte)4);

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Master",
                table: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                schema: "Master",
                table: "RoleUser",
                type: "VarChar(128)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                schema: "Master",
                table: "RolePermission",
                type: "VarChar(128)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PermissionName",
                schema: "Master",
                table: "RolePermission",
                type: "VarChar(128)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.AlterColumn<string>(
                name: "RelatedEntity",
                schema: "Master",
                table: "Permission",
                type: "VarChar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(128)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RelatedAggregateRoot",
                schema: "Master",
                table: "Permission",
                type: "VarChar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(128)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Master",
                table: "Permission",
                type: "VarChar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVarChar(128)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUser",
                schema: "Master",
                table: "RoleUser",
                columns: new[] { "RoleName", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "RoleName", "PermissionName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                schema: "Master",
                table: "Role",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                schema: "Master",
                table: "Permission",
                column: "Name");

            migrationBuilder.InsertData(
                schema: "Master",
                table: "Permission",
                columns: new[] { "Name", "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[,]
                {
                    { "Product_Read", null, "Product", "Product", "Read" },
                    { "Product_Read_Customer", "[\"Id\",\"Price\",\"Revision\",\"ProductName\",\"Reviews.Id\",\"Reviews.Description\",\"Reviews.Title\",\"Reviews.Username\",\"Reviews.Stars\",\"Reviews.CreatedOn\",\"Reviews.CreatedBy\",\"Reviews.UpdatedOn\",\"Reviews.UpdatedBy\"]", "Product", "Product", "Read" },
                    { "Review_Add", null, "Product", "Review", "Add" },
                    { "Review_Read", null, "Product", "Review", "Read" },
                    { "Review_Remove", null, "Product", "Review", "Remove" },
                    { "Review_Update", null, "Product", "Review", "Update" }
                });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "Role",
                column: "Name",
                values: new object[]
                {
                    "Administrator",
                    "Customer",
                    "Employee",
                    "Manager"
                });

            migrationBuilder.InsertData(
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "Review_Add", "Administrator" },
                    { "Review_Read", "Administrator" },
                    { "Review_Remove", "Administrator" },
                    { "Review_Update", "Administrator" },
                    { "Review_Add", "Customer" },
                    { "Review_Read", "Customer" },
                    { "Review_Update", "Customer" },
                    { "Review_Read", "Employee" },
                    { "Review_Add", "Manager" },
                    { "Review_Read", "Manager" },
                    { "Review_Update", "Manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionName",
                schema: "Master",
                table: "RolePermission",
                column: "PermissionName");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionName",
                schema: "Master",
                table: "RolePermission",
                column: "PermissionName",
                principalSchema: "Master",
                principalTable: "Permission",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleName",
                schema: "Master",
                table: "RolePermission",
                column: "RoleName",
                principalSchema: "Master",
                principalTable: "Role",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RoleName",
                schema: "Master",
                table: "RoleUser",
                column: "RoleName",
                principalSchema: "Master",
                principalTable: "Role",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionName",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Role_RoleName",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Role_RoleName",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleUser",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropIndex(
                name: "IX_RolePermission_PermissionName",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                schema: "Master",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Product_Read");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Product_Read_Customer");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Add", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Read", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Remove", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Update", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Add", "Customer" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Read", "Customer" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Update", "Customer" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Read", "Employee" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Add", "Manager" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Read", "Manager" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "RolePermission",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyColumnTypes: new[] { "VarChar(128)", "VarChar(128)" },
                keyValues: new object[] { "Review_Update", "Manager" });

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Review_Add");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Review_Read");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Review_Remove");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Name",
                keyValue: "Review_Update");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Name",
                keyValue: "Administrator");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Name",
                keyValue: "Customer");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Name",
                keyValue: "Employee");

            migrationBuilder.DeleteData(
                schema: "Master",
                table: "Role",
                keyColumn: "Name",
                keyValue: "Manager");

            migrationBuilder.DropColumn(
                name: "RoleName",
                schema: "Master",
                table: "RoleUser");

            migrationBuilder.DropColumn(
                name: "RoleName",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.AddColumn<byte>(
                name: "PermissionId",
                schema: "Master",
                table: "RolePermission",
                type: "TinyInt",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.DropColumn(
                name: "PermissionName",
                schema: "Master",
                table: "RolePermission");

            migrationBuilder.AddColumn<byte>(
                name: "RoleId",
                schema: "Master",
                table: "RoleUser",
                type: "TinyInt",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "RoleId",
                schema: "Master",
                table: "RolePermission",
                type: "TinyInt",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Id",
                schema: "Master",
                table: "Role",
                type: "TinyInt",
                nullable: false,
                defaultValue: (byte)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "RelatedEntity",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(128)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RelatedAggregateRoot",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(128)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(128)");

            migrationBuilder.AddColumn<byte>(
                name: "Id",
                schema: "Master",
                table: "Permission",
                type: "TinyInt",
                nullable: false,
                defaultValue: (byte)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleUser",
                schema: "Master",
                table: "RoleUser",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                schema: "Master",
                table: "RolePermission",
                columns: new[] { "RoleId", "PermissionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                schema: "Master",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                schema: "Master",
                table: "Permission",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "Master",
                table: "Permission",
                columns: new[] { "Id", "Name", "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[,]
                {
                    { (byte)1, "INVALID_PERMISSION", null, null, null, "Other" },
                    { (byte)2, "Review_Add", null, "Product", "Review", "Add" },
                    { (byte)3, "Review_Update", null, "Product", "Review", "Update" },
                    { (byte)4, "Review_Remove", null, "Product", "Review", "Remove" },
                    { (byte)5, "Review_Read", null, "Product", "Review", "Read" },
                    { (byte)6, "Product_Read", null, "Product", "Product", "Read" },
                    { (byte)7, "Product_Read_Customer", "[\"Id\",\"ProductName\",\"Price\",\"Revision\",\"Reviews.Id\",\"Reviews.Description\",\"Reviews.Title\",\"Reviews.Username\",\"Reviews.Stars\",\"Reviews.CreatedOn\",\"Reviews.CreatedBy\",\"Reviews.UpdatedOn\",\"Reviews.UpdatedBy\"]", "Product", "Product", "Read" }
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
                    { (byte)2, (byte)1 },
                    { (byte)3, (byte)1 },
                    { (byte)5, (byte)1 },
                    { (byte)5, (byte)2 },
                    { (byte)2, (byte)3 },
                    { (byte)3, (byte)3 },
                    { (byte)5, (byte)3 },
                    { (byte)2, (byte)4 },
                    { (byte)3, (byte)4 },
                    { (byte)4, (byte)4 },
                    { (byte)5, (byte)4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Master",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                schema: "Master",
                table: "Permission",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                schema: "Master",
                table: "RolePermission",
                column: "PermissionId",
                principalSchema: "Master",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Role_RoleId",
                schema: "Master",
                table: "RolePermission",
                column: "RoleId",
                principalSchema: "Master",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Role_RoleId",
                schema: "Master",
                table: "RoleUser",
                column: "RoleId",
                principalSchema: "Master",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
