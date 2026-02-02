using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPermissions2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogin", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogin", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogin", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserRefreshToken", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserRefreshToken", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserRefreshToken", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserLogin");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserRefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionName", "Description" },
                values: new object[,]
                {
                    { "UserLogin", "UserLogin" },
                    { "UserRefreshToken", "UserRefreshToken" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "UserLogin", "Admin" },
                    { "UserLogin", "HR" },
                    { "UserLogin", "SuperHR" },
                    { "UserRefreshToken", "Admin" },
                    { "UserRefreshToken", "HR" },
                    { "UserRefreshToken", "SuperHR" }
                });
        }
    }
}
