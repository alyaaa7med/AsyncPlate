using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncPlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshTokenLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductExtra_Products_ExtraProductId",
                table: "ProductExtra");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductExtra_Products_ProductId",
                table: "ProductExtra");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductExtra",
                table: "ProductExtra");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "ProductExtra",
                newName: "ProductExtras");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductExtra_ExtraProductId",
                table: "ProductExtras",
                newName: "IX_ProductExtras_ExtraProductId");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshTokenValue",
                table: "RefreshTokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductExtras",
                table: "ProductExtras",
                columns: new[] { "ProductId", "ExtraProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductExtras_Products_ExtraProductId",
                table: "ProductExtras",
                column: "ExtraProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductExtras_Products_ProductId",
                table: "ProductExtras",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductExtras_Products_ExtraProductId",
                table: "ProductExtras");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductExtras_Products_ProductId",
                table: "ProductExtras");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductExtras",
                table: "ProductExtras");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "ProductExtras",
                newName: "ProductExtra");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductExtras_ExtraProductId",
                table: "ProductExtra",
                newName: "IX_ProductExtra_ExtraProductId");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshTokenValue",
                table: "RefreshToken",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductExtra",
                table: "ProductExtra",
                columns: new[] { "ProductId", "ExtraProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductExtra_Products_ExtraProductId",
                table: "ProductExtra",
                column: "ExtraProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductExtra_Products_ProductId",
                table: "ProductExtra",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
