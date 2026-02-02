using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_CreatorId",
                table: "AccountCreationToken");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "AccountCreationToken",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_CreatorId",
                table: "AccountCreationToken",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_CreatorId",
                table: "AccountCreationToken");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "AccountCreationToken",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_CreatorId",
                table: "AccountCreationToken",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
