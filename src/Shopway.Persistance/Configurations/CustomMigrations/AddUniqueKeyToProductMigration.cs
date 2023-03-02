using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopway.Persistence.Migrations;

public partial class Add_Unique_Key_To_Product : Migration
{
    private const string CreateIndex = "CREATE UNIQUE INDEX IX_Product_ProductName_Revision ON [Shopway].[Product](ProductName, Revision);";
    private const string DropIndex = "DROP INDEX IX_Product_ProductName_Revision ON [Shopway].[Product];";

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"{CreateIndex}");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"{DropIndex}");
    }
}