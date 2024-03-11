using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRetryStrategyForOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage");

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExecutionStatus",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "VarChar(10)",
                nullable: false,
                defaultValue: "InProgress");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "NextProcessAttempt",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExecutionStatus",
                schema: "Outbox",
                table: "OutboxMessage",
                column: "ExecutionStatus",
                filter: "[ExecutionStatus] = 'InProgress'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_ExecutionStatus",
                schema: "Outbox",
                table: "OutboxMessage");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                schema: "Outbox",
                table: "OutboxMessage");

            migrationBuilder.DropColumn(
                name: "ExecutionStatus",
                schema: "Outbox",
                table: "OutboxMessage");

            migrationBuilder.DropColumn(
                name: "NextProcessAttempt",
                schema: "Outbox",
                table: "OutboxMessage");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage",
                column: "ProcessedOn",
                filter: "[ProcessedOn] IS NULL");
        }
    }
}
