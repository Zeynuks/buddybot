using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPermissions4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateHr", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenRevoke", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "DepartmentCreate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "DepartmentDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "DepartmentUpdate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "FeedbackDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamCreate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamUpdate", "HR" });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "DepartmentView", "HR" },
                    { "FeedbackView", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "DepartmentView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "FeedbackView", "HR" });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreateHr", "HR" },
                    { "AccountCreationTokenDelete", "HR" },
                    { "AccountCreationTokenRevoke", "HR" },
                    { "DepartmentCreate", "HR" },
                    { "DepartmentDelete", "HR" },
                    { "DepartmentUpdate", "HR" },
                    { "FeedbackDelete", "HR" },
                    { "TeamCreate", "HR" },
                    { "TeamDelete", "HR" },
                    { "TeamUpdate", "HR" }
                });
        }
    }
}
