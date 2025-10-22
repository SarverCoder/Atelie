using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atelie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropOrderWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Monogram",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "OrderWorkers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "OrderWorkers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Monogram",
                table: "Orders",
                type: "text",
                nullable: true);
        }
    }
}
