using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrifoodManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedForumCategoryContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPosts_ForumThreads_ThreadId",
                table: "ForumPosts");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "ForumThreads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "ForumThreads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThreads_CreatedByUserId",
                table: "ForumThreads",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPosts_ForumThreads_ThreadId",
                table: "ForumPosts",
                column: "ThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThreads_Users_CreatedByUserId",
                table: "ForumThreads",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPosts_ForumThreads_ThreadId",
                table: "ForumPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumThreads_Users_CreatedByUserId",
                table: "ForumThreads");

            migrationBuilder.DropIndex(
                name: "IX_ForumThreads_CreatedByUserId",
                table: "ForumThreads");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "ForumThreads");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "ForumThreads");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPosts_ForumThreads_ThreadId",
                table: "ForumPosts",
                column: "ThreadId",
                principalTable: "ForumThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
