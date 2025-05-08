using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrifoodManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscountAddressOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLatitude",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLongitude",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountCodeId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DiscountCodeId",
                table: "Orders",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DiscountCodes_DiscountCodeId",
                table: "Orders",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DiscountCodes_DiscountCodeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DiscountCodeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLatitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLongitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Orders");
        }
    }
}
