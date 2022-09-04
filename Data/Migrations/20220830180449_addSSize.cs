using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addSSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9abc32ec-a202-4310-a103-452fdfb1d046", "b269d289-b839-4241-9257-208a52ff6b46" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9abc32ec-a202-4310-a103-452fdfb1d046");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b269d289-b839-4241-9257-208a52ff6b46");

            migrationBuilder.AddColumn<string>(
                name: "Sizes",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductInOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "24687802-e3e6-446f-a358-f2c44abc5f51", "759f0d84-0124-4bcd-bf2b-bf0c5d6cd6b8", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0e6a5d43-b6b2-4cd0-b7ac-d81710645ef0", 0, "f7689327-feaf-4f44-8bfb-a539060cbb4e", null, false, "ادمین", false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEHXU4mA4uyT8bn/W7owKZIAKUIT6pFfBdsfKbl8HVpV5NnF1SUKVp/ZKMOSwVHTWKw==", null, false, "f8a7bc2b-8ed0-4c88-95c9-8d77dcc21f00", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "24687802-e3e6-446f-a358-f2c44abc5f51", "0e6a5d43-b6b2-4cd0-b7ac-d81710645ef0" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInOrders_OrderId",
                table: "ProductInOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInOrders_ProductId",
                table: "ProductInOrders",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductInOrders");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "24687802-e3e6-446f-a358-f2c44abc5f51", "0e6a5d43-b6b2-4cd0-b7ac-d81710645ef0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24687802-e3e6-446f-a358-f2c44abc5f51");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e6a5d43-b6b2-4cd0-b7ac-d81710645ef0");

            migrationBuilder.DropColumn(
                name: "Sizes",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9abc32ec-a202-4310-a103-452fdfb1d046", "8579fe83-5cf1-4a91-b7da-ec619bbd3ba5", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b269d289-b839-4241-9257-208a52ff6b46", 0, "e922daee-d1b2-441b-9e1c-71bded921ff5", null, false, "ادمین", false, null, null, "ADMIN", "AQAAAAEAACcQAAAAENLqeoqf5gjAHLRi4iQ3iXpLjjOeFEsPPri54tR8M3bEjIHUsG4H7guC0IkCG/mSyA==", null, false, "e8e94c25-c0dd-4684-b537-bc9b5b2a90e4", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9abc32ec-a202-4310-a103-452fdfb1d046", "b269d289-b839-4241-9257-208a52ff6b46" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductsId",
                table: "OrderProduct",
                column: "ProductsId");
        }
    }
}
