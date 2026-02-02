using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdInAccountCreationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AccountCreationToken",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountCreationToken_UserId",
                table: "AccountCreationToken",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_UserId",
                table: "AccountCreationToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_UserId",
                table: "AccountCreationToken");

            migrationBuilder.DropIndex(
                name: "IX_AccountCreationToken_UserId",
                table: "AccountCreationToken");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AccountCreationToken");
        }
    }
}
