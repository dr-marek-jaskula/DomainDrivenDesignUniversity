using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetSparseColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OccurredOn",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "DateTimeOffset(2)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "NextProcessAttempt",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "DateTimeOffset(2)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "VarChar(8000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(8000)",
                oldNullable: true)
                .Annotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<byte>(
                name: "AttemptCount",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "TinyInt",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ProcessedOn",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OccurredOn",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "NextProcessAttempt",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "DateTimeOffset(2)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<string>(
                name: "Error",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "VarChar(8000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VarChar(8000)",
                oldNullable: true)
                .OldAnnotation("SqlServer:Sparse", true);

            migrationBuilder.AlterColumn<int>(
                name: "AttemptCount",
                schema: "Outbox",
                table: "OutboxMessage",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "TinyInt");
        }
    }
}
