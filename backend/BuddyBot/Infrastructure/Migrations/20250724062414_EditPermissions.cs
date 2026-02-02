using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateCreate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateCreate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateCreate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateUpdate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateUpdate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateUpdate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateView", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateView", "Mentor" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CandidateView", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityCreate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityCreate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityUpdate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityUpdate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityView", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CityView", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryCreate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryCreate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryUpdate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryUpdate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryView", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "CountryView", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "FeedbackCreate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "FeedbackCreate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserCreateHR", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserCreateHR", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserRegister", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserRegister", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenCreate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CandidateCreate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CandidateUpdate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CandidateView");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CityCreate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CityDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CityUpdate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CityView");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CountryCreate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CountryDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CountryUpdate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "CountryView");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "FeedbackCreate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserCreateHR");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserRegister");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenUpdate",
                column: "Description",
                value: "AccountCreationTokenUpdate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenView",
                column: "Description",
                value: "AccountCreationTokenView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentCreate",
                column: "Description",
                value: "DepartmentCreate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentDelete",
                column: "Description",
                value: "DepartmentDelete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentUpdate",
                column: "Description",
                value: "DepartmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentView",
                column: "Description",
                value: "DepartmentView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "FeedbackDelete",
                column: "Description",
                value: "FeedbackDelete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "FeedbackView",
                column: "Description",
                value: "FeedbackView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestConfirm",
                column: "Description",
                value: "OnboardingAccessRequestConfirm");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestCreate",
                column: "Description",
                value: "OnboardingAccessRequestCreate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestReject",
                column: "Description",
                value: "OnboardingAccessRequestReject");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestUpdate",
                column: "Description",
                value: "OnboardingAccessRequestUpdate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestView",
                column: "Description",
                value: "OnboardingAccessRequestView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamCreate",
                column: "Description",
                value: "TeamCreate");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamView",
                column: "Description",
                value: "TeamView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserCreateMentor",
                column: "Description",
                value: "UserCreateMentor");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserLogin",
                column: "Description",
                value: "UserLogin");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserRefreshToken",
                column: "Description",
                value: "UserRefreshToken");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionName", "Description" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreateCandidate", "AccountCreationTokenCreateCandidate" },
                    { "AccountCreationTokenCreateHr", "AccountCreationTokenCreateHr" },
                    { "AccountCreationTokenDelete", "AccountCreationTokenDelete" },
                    { "AccountCreationTokenRevoke", "AccountCreationTokenRevoke" },
                    { "OnboardingAccessRequestDelete", "OnboardingAccessRequestDelete" },
                    { "OnboardingAccessRequestRestore", "OnboardingAccessRequestRestore" },
                    { "TeamDelete", "TeamDelete" },
                    { "TeamUpdate", "TeamUpdate" },
                    { "UserDelete", "UserDelete" },
                    { "UserLogout", "UserLogout" },
                    { "UserUpdate", "UserUpdate" },
                    { "UserView", "UserView" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "DepartmentCreate", "HR" },
                    { "DepartmentDelete", "HR" },
                    { "DepartmentUpdate", "HR" },
                    { "FeedbackDelete", "HR" },
                    { "TeamCreate", "HR" },
                    { "AccountCreationTokenCreateCandidate", "Admin" },
                    { "AccountCreationTokenCreateCandidate", "HR" },
                    { "AccountCreationTokenCreateCandidate", "SuperHR" },
                    { "AccountCreationTokenCreateHr", "Admin" },
                    { "AccountCreationTokenCreateHr", "HR" },
                    { "AccountCreationTokenCreateHr", "SuperHR" },
                    { "AccountCreationTokenDelete", "Admin" },
                    { "AccountCreationTokenDelete", "HR" },
                    { "AccountCreationTokenDelete", "SuperHR" },
                    { "AccountCreationTokenRevoke", "Admin" },
                    { "AccountCreationTokenRevoke", "HR" },
                    { "AccountCreationTokenRevoke", "SuperHR" },
                    { "OnboardingAccessRequestDelete", "Admin" },
                    { "OnboardingAccessRequestDelete", "HR" },
                    { "OnboardingAccessRequestDelete", "SuperHR" },
                    { "OnboardingAccessRequestRestore", "Admin" },
                    { "OnboardingAccessRequestRestore", "HR" },
                    { "OnboardingAccessRequestRestore", "SuperHR" },
                    { "TeamDelete", "Admin" },
                    { "TeamDelete", "HR" },
                    { "TeamDelete", "SuperHR" },
                    { "TeamUpdate", "Admin" },
                    { "TeamUpdate", "HR" },
                    { "TeamUpdate", "SuperHR" },
                    { "UserDelete", "Admin" },
                    { "UserDelete", "HR" },
                    { "UserDelete", "SuperHR" },
                    { "UserLogout", "Admin" },
                    { "UserLogout", "HR" },
                    { "UserLogout", "SuperHR" },
                    { "UserUpdate", "Admin" },
                    { "UserUpdate", "HR" },
                    { "UserUpdate", "SuperHR" },
                    { "UserView", "Admin" },
                    { "UserView", "HR" },
                    { "UserView", "SuperHR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateCandidate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateCandidate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateCandidate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateHr", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateHr", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenCreateHr", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenRevoke", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenRevoke", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "AccountCreationTokenRevoke", "SuperHR" });

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
                keyValues: new object[] { "OnboardingAccessRequestDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "OnboardingAccessRequestDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "OnboardingAccessRequestDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "OnboardingAccessRequestRestore", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "OnboardingAccessRequestRestore", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "OnboardingAccessRequestRestore", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamCreate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamUpdate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamUpdate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "TeamUpdate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserDelete", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserDelete", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserDelete", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogout", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogout", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserLogout", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserUpdate", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserUpdate", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserUpdate", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserView", "Admin" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserView", "HR" });

            migrationBuilder.DeleteData(
                table: "PermissionRole",
                keyColumns: new[] { "PermissionName", "RoleName" },
                keyValues: new object[] { "UserView", "SuperHR" });

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenCreateCandidate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenCreateHr");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenRevoke");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestRestore");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamUpdate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserDelete");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserLogout");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserUpdate");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserView");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenUpdate",
                column: "Description",
                value: "Редактирование токена для создание кандидата");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "AccountCreationTokenView",
                column: "Description",
                value: "Просмотр токенов для создания кандидатов");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentCreate",
                column: "Description",
                value: "Создание отдела");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentDelete",
                column: "Description",
                value: "Удаление отдела");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentUpdate",
                column: "Description",
                value: "Редактирование отдела");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "DepartmentView",
                column: "Description",
                value: "Просмотр отделов");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "FeedbackDelete",
                column: "Description",
                value: "Удаление отзыва");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "FeedbackView",
                column: "Description",
                value: "Просмотр отзывов");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestConfirm",
                column: "Description",
                value: "Подтверждение заявки на доступ к онбордингу");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestCreate",
                column: "Description",
                value: "Создание заявки на доступ к онбордингу");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestReject",
                column: "Description",
                value: "Отклонение заявки на доступ К онбордингу");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestUpdate",
                column: "Description",
                value: "Обновление данных заявки на доступ к онбординге");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "OnboardingAccessRequestView",
                column: "Description",
                value: "Просмотр заявок на доступ к онбордингу");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamCreate",
                column: "Description",
                value: "Создание команды");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "TeamView",
                column: "Description",
                value: "Просмотр команд");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserCreateMentor",
                column: "Description",
                value: "Создание наставника");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserLogin",
                column: "Description",
                value: "Вход пользователя");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "PermissionName",
                keyValue: "UserRefreshToken",
                column: "Description",
                value: "Обновление токена доступа");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionName", "Description" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreate", "Генерация токена для создания кандидата" },
                    { "CandidateCreate", "Создание кандидата" },
                    { "CandidateUpdate", "Обновление данных кандидата" },
                    { "CandidateView", "Просмотр кандидатов" },
                    { "CityCreate", "Создание города" },
                    { "CityDelete", "Удаление города" },
                    { "CityUpdate", "Редактирование города" },
                    { "CityView", "Просмотр городов" },
                    { "CountryCreate", "Создание страны" },
                    { "CountryDelete", "Удаление страны" },
                    { "CountryUpdate", "Редактирование страны" },
                    { "CountryView", "Просмотр стран" },
                    { "FeedbackCreate", "Добавление отзыва" },
                    { "UserCreateHR", "Создание HR-пользователя" },
                    { "UserRegister", "Регистрация пользователя" }
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreate", "Admin" },
                    { "AccountCreationTokenCreate", "HR" },
                    { "AccountCreationTokenCreate", "SuperHR" },
                    { "CandidateCreate", "Admin" },
                    { "CandidateCreate", "HR" },
                    { "CandidateCreate", "SuperHR" },
                    { "CandidateUpdate", "Admin" },
                    { "CandidateUpdate", "HR" },
                    { "CandidateUpdate", "SuperHR" },
                    { "CandidateView", "Admin" },
                    { "CandidateView", "HR" },
                    { "CandidateView", "Mentor" },
                    { "CandidateView", "SuperHR" },
                    { "CityCreate", "Admin" },
                    { "CityCreate", "SuperHR" },
                    { "CityDelete", "Admin" },
                    { "CityDelete", "SuperHR" },
                    { "CityUpdate", "Admin" },
                    { "CityUpdate", "SuperHR" },
                    { "CityView", "Admin" },
                    { "CityView", "HR" },
                    { "CityView", "SuperHR" },
                    { "CountryCreate", "Admin" },
                    { "CountryCreate", "SuperHR" },
                    { "CountryDelete", "Admin" },
                    { "CountryDelete", "SuperHR" },
                    { "CountryUpdate", "Admin" },
                    { "CountryUpdate", "SuperHR" },
                    { "CountryView", "Admin" },
                    { "CountryView", "HR" },
                    { "CountryView", "SuperHR" },
                    { "FeedbackCreate", "Admin" },
                    { "FeedbackCreate", "SuperHR" },
                    { "UserCreateHR", "Admin" },
                    { "UserCreateHR", "SuperHR" },
                    { "UserRegister", "Admin" },
                    { "UserRegister", "SuperHR" }
                });
        }
    }
}
