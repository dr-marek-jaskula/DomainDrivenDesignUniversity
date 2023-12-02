using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedOnFilteredIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage",
                column: "ProcessedOn",
                filter: "[ProcessedOn] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage");
        }
    }
}
