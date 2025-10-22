using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "AdvancePayment",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Orders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ДГ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ДЛ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ДТП2",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ДТС",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ОБ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ОГ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ОТ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ПЛ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Ру",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ШГ",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ШС",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Шбс",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Шн",
                table: "Orders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FabricId",
                table: "OrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems",
                column: "FabricId",
                principalTable: "Fabrics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "AdvancePayment",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ДГ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ДЛ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ДТП2",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ДТС",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ОБ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ОГ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ОТ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ПЛ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Ру",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ШГ",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ШС",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Шбс",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Шн",
                table: "Orders");

            migrationBuilder.AlterColumn<long>(
                name: "FabricId",
                table: "OrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Fabrics_FabricId",
                table: "OrderItems",
                column: "FabricId",
                principalTable: "Fabrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
