using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCandidateToUserInAccountCreationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_CandidateId",
                table: "AccountCreationToken");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "AccountCreationToken",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountCreationToken_CandidateId",
                table: "AccountCreationToken",
                newName: "IX_AccountCreationToken_UserId");

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

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AccountCreationToken",
                newName: "CandidateId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountCreationToken_UserId",
                table: "AccountCreationToken",
                newName: "IX_AccountCreationToken_CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_CandidateId",
                table: "AccountCreationToken",
                column: "CandidateId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
