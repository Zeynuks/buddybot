using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPermissionsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[] { "AccountCreationTokenRevoke", "HR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenRevoke", "HR" });
        }
    }
}
