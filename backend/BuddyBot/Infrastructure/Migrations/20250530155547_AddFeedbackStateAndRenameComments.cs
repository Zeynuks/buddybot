using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackStateAndRenameComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Feedback",
                newName: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Feedback",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Feedback",
                newName: "Comments");
        }
    }
}
