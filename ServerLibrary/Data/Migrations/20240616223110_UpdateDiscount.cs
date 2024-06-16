using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_ApplicationUsers_CustomerId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUsers_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_CustomerId",
                table: "ProductReviews");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_CustomerId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ProductReviews",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_CustomerId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.CreateTable(
                name: "DiscountWarehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountWarehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountWarehouses_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountWarehouses_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountWarehouses_DiscountId",
                table: "DiscountWarehouses",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountWarehouses_UserId",
                table: "DiscountWarehouses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_UserId",
                table: "ProductReviews",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ApplicationUsers_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_UserId",
                table: "ProductReviews");

            migrationBuilder.DropTable(
                name: "DiscountWarehouses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProductReviews",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_UserId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_CustomerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_CustomerId",
                table: "Discounts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_ApplicationUsers_CustomerId",
                table: "Discounts",
                column: "CustomerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ApplicationUsers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_ApplicationUsers_CustomerId",
                table: "ProductReviews",
                column: "CustomerId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
