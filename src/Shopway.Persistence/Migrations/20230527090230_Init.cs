using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shopway.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Master");

            migrationBuilder.EnsureSchema(
                name: "Shopway");

            migrationBuilder.EnsureSchema(
                name: "Outbox");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "VarChar(7)", nullable: false),
                    Rank = table.Column<string>(type: "VarChar(8)", nullable: false, defaultValue: "Standard"),
                    DateOfBirth = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "Outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "VarChar(100)", nullable: false),
                    Content = table.Column<string>(type: "VarChar(5000)", nullable: false),
                    OccurredOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Error = table.Column<string>(type: "VarChar(8000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessageConsumer",
                schema: "Outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageConsumer", x => new { x.Id, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "Shopway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Status = table.Column<string>(type: "VarChar(11)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true),
                    OrderHeaderId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "TinyInt", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VarChar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Shopway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Revision = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    UomCode = table.Column<string>(type: "VarChar(8)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "TinyInt", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VarChar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Building = table.Column<int>(type: "int", maxLength: 1000, nullable: false),
                    Flat = table.Column<int>(type: "int", maxLength: 1000, nullable: true),
                    CustomerId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Master",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Master",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true),
                    PasswordHash = table.Column<string>(type: "NChar(514)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "UniqueIdentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Master",
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                schema: "Shopway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Stars = table.Column<byte>(type: "TinyInt", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    ProductId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shopway",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "Master",
                columns: table => new
                {
                    RoleId = table.Column<byte>(type: "TinyInt", nullable: false),
                    PermissionId = table.Column<byte>(type: "TinyInt", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Master",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Master",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeader",
                schema: "Shopway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "VarChar(10)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true),
                    PaymentId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeader_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalSchema: "Shopway",
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderHeader_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Master",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "Master",
                columns: table => new
                {
                    RoleId = table.Column<byte>(type: "TinyInt", nullable: false),
                    UserId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Master",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Master",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLine",
                schema: "Shopway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "DateTimeOffset(2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "VarChar(30)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VarChar(30)", nullable: true),
                    ProductId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OrderHeaderId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLine_OrderHeader_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalSchema: "Shopway",
                        principalTable: "OrderHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLine_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Shopway",
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Address_CustomerId",
                schema: "Master",
                table: "Address",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_PaymentId",
                schema: "Shopway",
                table: "OrderHeader",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeader_UserId",
                schema: "Shopway",
                table: "OrderHeader",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_OrderHeaderId",
                schema: "Shopway",
                table: "OrderLine",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_ProductId",
                schema: "Shopway",
                table: "OrderLine",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_ProductId",
                schema: "Shopway",
                table: "Review",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Master",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                schema: "Master",
                table: "RoleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerId",
                schema: "Master",
                table: "User",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_User_Email",
                schema: "Master",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Username_Email",
                schema: "Master",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "OrderLine",
                schema: "Shopway");

            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "Outbox");

            migrationBuilder.DropTable(
                name: "OutboxMessageConsumer",
                schema: "Outbox");

            migrationBuilder.DropTable(
                name: "Review",
                schema: "Shopway");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "OrderHeader",
                schema: "Shopway");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Shopway");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Shopway");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Master");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Master");
        }
    }
}
