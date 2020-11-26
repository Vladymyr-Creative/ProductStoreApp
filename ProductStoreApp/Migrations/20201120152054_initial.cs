using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductStoreApp.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,9)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PriceBase = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Product_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProductStore",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStore", x => new { x.ProductId, x.StoreId });
                    table.ForeignKey(
                        name: "FK_ProductStore_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStore_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Food" },
                    { 2, "Sport" },
                    { 3, "Health" }
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "Code", "Name", "Rate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 840, "USD - Долар США", 1m, new DateTime(2020, 11, 19, 17, 20, 54, 537, DateTimeKind.Local).AddTicks(6059) },
                    { 2, 978, "EUR - Євро", 0.8m, new DateTime(2020, 11, 19, 17, 20, 54, 538, DateTimeKind.Local).AddTicks(7695) },
                    { 3, 980, "UAH - Українська Гривня", 25m, new DateTime(2020, 11, 19, 17, 20, 54, 538, DateTimeKind.Local).AddTicks(7736) }
                });

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { 1, "Address1", "Store1" },
                    { 2, "Address2", "Store2" },
                    { 3, "Address3", "Store3" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Code", "CurrencyId", "Name", "Price", "PriceBase" },
                values: new object[] { 1, 1, "10000001", 1, "Orange", 1m, 1m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Code", "CurrencyId", "Name", "Price", "PriceBase" },
                values: new object[] { 2, 2, "10000002", 1, "Ball", 2m, 2m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CategoryId", "Code", "CurrencyId", "Name", "Price", "PriceBase" },
                values: new object[] { 3, 3, "10000003", 1, "Vitamin C", 3m, 3m });

            migrationBuilder.InsertData(
                table: "ProductStore",
                columns: new[] { "ProductId", "StoreId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 11 },
                    { 1, 2, 12 },
                    { 1, 3, 13 },
                    { 2, 1, 21 },
                    { 2, 2, 22 },
                    { 2, 3, 23 },
                    { 3, 1, 31 },
                    { 3, 2, 32 },
                    { 3, 3, 33 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Code",
                table: "Currency",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Code",
                table: "Product",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_CurrencyId",
                table: "Product",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStore_StoreId",
                table: "ProductStore",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Name_Address",
                table: "Store",
                columns: new[] { "Name", "Address" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStore");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
