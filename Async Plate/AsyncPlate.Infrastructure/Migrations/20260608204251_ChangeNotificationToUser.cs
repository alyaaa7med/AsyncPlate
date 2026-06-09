using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncPlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNotificationToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Customers_CustomerId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Notifications",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_CustomerId",
                table: "Notifications",
                newName: "IX_Notifications_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_userId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Notifications",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_userId",
                table: "Notifications",
                newName: "IX_Notifications_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Customers_CustomerId",
                table: "Notifications",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
