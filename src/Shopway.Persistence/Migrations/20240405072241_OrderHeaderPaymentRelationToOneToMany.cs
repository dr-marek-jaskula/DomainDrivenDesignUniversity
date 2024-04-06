using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderHeaderPaymentRelationToOneToMany : Migration
    {
        private const string _migratePaymentIdSql = """
            UPDATE [Shopway].[Payment]
            SET OrderHeaderId = (
                SELECT TOP (1) [Id] 
                FROM [Shopway].[OrderHeader] 
                WHERE [PaymentId] = [Shopway].[Payment].[Id]
            );
            """;

        private const string _deleteInvalidPayments = """
            DELETE FROM [Shopway].[Payment]
            WHERE OrderHeaderId IS NULL OR OrderHeaderId = ''
            """;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderHeaderId",
                schema: "Shopway",
                table: "Payment",
                type: "Char(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(_migratePaymentIdSql);
            migrationBuilder.Sql(_deleteInvalidPayments);

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeader_Payment_PaymentId",
                schema: "Shopway",
                table: "OrderHeader");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeader_PaymentId",
                schema: "Shopway",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "Shopway",
                table: "OrderHeader");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderHeaderId",
                schema: "Shopway",
                table: "Payment",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_OrderHeader_OrderHeaderId",
                schema: "Shopway",
                table: "Payment",
                column: "OrderHeaderId",
                principalSchema: "Shopway",
                principalTable: "OrderHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_OrderHeader_OrderHeaderId",
                schema: "Shopway",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_OrderHeaderId",
                schema: "Shopway",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "OrderHeaderId",
                schema: "Shopway",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                schema: "Shopway",
                table: "OrderHeader",
                type: "Char(26)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_PaymentId",
                schema: "Shopway",
                table: "OrderHeader",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeader_Payment_PaymentId",
                schema: "Shopway",
                table: "OrderHeader",
                column: "PaymentId",
                principalSchema: "Shopway",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
