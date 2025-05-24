using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrifoodManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedReviewAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewedUserId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewedUserId",
                table: "Reviews",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewedUserId",
                table: "Reviews",
                newName: "IX_Reviews_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Products_ProductId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Reviews",
                newName: "ReviewedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewedUserId",
                table: "Reviews",
                column: "ReviewedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
