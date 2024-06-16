using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExtendPermissionEnumeration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(128)");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                schema: "Master",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedAggregateRoot",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntity",
                schema: "Master",
                table: "Permission",
                type: "NVarChar(128)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "Master",
                table: "Permission",
                type: "VarChar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)1,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, null, null, "Other" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)2,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, "Product", "Review", "Add" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)3,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, "Product", "Review", "Update" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)4,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, "Product", "Review", "Remove" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)5,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, "Product", "Review", "Read" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)6,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { null, "Product", null, "Read" });

            migrationBuilder.UpdateData(
                schema: "Master",
                table: "Permission",
                keyColumn: "Id",
                keyValue: (byte)7,
                columns: new[] { "Properties", "RelatedAggregateRoot", "RelatedEntity", "Type" },
                values: new object[] { "[\"Id\",\"ProductName\",\"Price\",\"Revision\",\"Reviews.Id\",\"Reviews.Description\",\"Reviews.Title\",\"Reviews.Username\",\"Reviews.Stars\",\"Reviews.CreatedOn\",\"Reviews.CreatedBy\",\"Reviews.UpdatedOn\",\"Reviews.UpdatedBy\"]", "Product", null, "Read" });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                schema: "Master",
                table: "Permission",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permission_Name",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Properties",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RelatedAggregateRoot",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RelatedEntity",
                schema: "Master",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Master",
                table: "Permission");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Master",
                table: "Permission",
                type: "VarChar(128)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVarChar(128)");
        }
    }
}
