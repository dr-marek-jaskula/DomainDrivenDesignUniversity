using Microsoft.EntityFrameworkCore.Migrations;

namespace Shopway.Persistence.Migrations;

public partial class Add_Unique_Key_To_Review : Migration
{
    private const string CreateIndex = "CREATE UNIQUE INDEX IX_Review_ProductId_Title ON [Shopway].[Review](ProductId, Title);";
    private const string DropIndex = "DROP INDEX IX_Review_ProductId_Title ON [Shopway].[Review];";

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        //migrationBuilder.Sql($"{CreateIndex}");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        //migrationBuilder.Sql($"{DropIndex}");
    }
}