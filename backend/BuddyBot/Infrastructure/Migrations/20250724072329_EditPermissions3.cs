using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPermissions3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "DepartmentView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "FeedbackView", "HR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "DepartmentView", "HR" },
                    { "FeedbackView", "HR" }
                });
        }
    }
}
