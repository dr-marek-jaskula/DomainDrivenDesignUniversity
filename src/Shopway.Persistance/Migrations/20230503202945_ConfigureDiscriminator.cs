using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "Workflow",
                table: "WorkItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "VarChar(8000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(5000)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                schema: "Workflow",
                table: "WorkItem",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "VarChar(5000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(8000)",
                oldNullable: true);
        }
    }
}
