using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFabricQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FabricQuantity",
                table: "OrderItems",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FabricQuantity",
                table: "OrderItems");
        }
    }
}
