using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsyncPlate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductExtraTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductExtras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ProductExtras",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
