using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgrifoodManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedSentimentDatapoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SentimentConfidence",
                table: "Reviews",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "SentimentType",
                table: "Reviews",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentimentConfidence",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "SentimentType",
                table: "Reviews");
        }
    }
}
