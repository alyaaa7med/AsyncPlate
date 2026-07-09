using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncPlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingextraitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderExtraItem_OrderItems_OrderItemId",
                table: "OrderExtraItem");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderExtraItem_Products_ProductId",
                table: "OrderExtraItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderExtraItem",
                table: "OrderExtraItem");

            migrationBuilder.RenameTable(
                name: "OrderExtraItem",
                newName: "OrderExtraItems");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExtraItem_ProductId",
                table: "OrderExtraItems",
                newName: "IX_OrderExtraItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExtraItem_OrderItemId",
                table: "OrderExtraItems",
                newName: "IX_OrderExtraItems_OrderItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderExtraItems",
                table: "OrderExtraItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExtraItems_OrderItems_OrderItemId",
                table: "OrderExtraItems",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExtraItems_Products_ProductId",
                table: "OrderExtraItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderExtraItems_OrderItems_OrderItemId",
                table: "OrderExtraItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderExtraItems_Products_ProductId",
                table: "OrderExtraItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderExtraItems",
                table: "OrderExtraItems");

            migrationBuilder.RenameTable(
                name: "OrderExtraItems",
                newName: "OrderExtraItem");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExtraItems_ProductId",
                table: "OrderExtraItem",
                newName: "IX_OrderExtraItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderExtraItems_OrderItemId",
                table: "OrderExtraItem",
                newName: "IX_OrderExtraItem_OrderItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderExtraItem",
                table: "OrderExtraItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExtraItem_OrderItems_OrderItemId",
                table: "OrderExtraItem",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderExtraItem_Products_ProductId",
                table: "OrderExtraItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
