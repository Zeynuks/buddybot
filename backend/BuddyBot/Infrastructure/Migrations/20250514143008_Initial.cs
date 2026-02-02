using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HeadFirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeadLastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HeadVideoUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HeadMicrosoftTeamsUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionName);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleName);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Login = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionName = table.Column<string>(type: "text", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionName, x.RoleName });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permission_PermissionName",
                        column: x => x.PermissionName,
                        principalTable: "Permission",
                        principalColumn: "PermissionName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Role_RoleName",
                        column: x => x.RoleName,
                        principalTable: "Role",
                        principalColumn: "RoleName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_HRInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesRoleName = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesRoleName, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesRoleName",
                        column: x => x.RolesRoleName,
                        principalTable: "Role",
                        principalColumn: "RoleName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LeaderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_User_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAuthToken",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthToken", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserAuthToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContactInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MentorVideoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MentorPhotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MicrosoftTeamsUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TelegramContact = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserContactInfo_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamId = table.Column<int>(type: "integer", nullable: true),
                    OnboardingAccessTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidate_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeamUser",
                columns: table => new
                {
                    MembersId = table.Column<int>(type: "integer", nullable: false),
                    TeamsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUser", x => new { x.MembersId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_TeamUser_Team_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamUser_User_MembersId",
                        column: x => x.MembersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountCreationToken",
                columns: table => new
                {
                    TokenValue = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActivatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TelegramInviteLink = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    QrCodeBase64 = table.Column<string>(type: "text", nullable: true),
                    CandidateId = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCreationToken", x => x.TokenValue);
                    table.ForeignKey(
                        name: "FK_AccountCreationToken_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccountCreationToken_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateContactInfo",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateContactInfo", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_CandidateContactInfo_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateContactInfo_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CandidateHR",
                columns: table => new
                {
                    HRCandidatesId = table.Column<int>(type: "integer", nullable: false),
                    HRsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateHR", x => new { x.HRCandidatesId, x.HRsId });
                    table.ForeignKey(
                        name: "FK_CandidateHR_Candidate_HRCandidatesId",
                        column: x => x.HRCandidatesId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateHR_User_HRsId",
                        column: x => x.HRsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateMentor",
                columns: table => new
                {
                    MentoredCandidatesId = table.Column<int>(type: "integer", nullable: false),
                    MentorsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateMentor", x => new { x.MentoredCandidatesId, x.MentorsId });
                    table.ForeignKey(
                        name: "FK_CandidateMentor_Candidate_MentoredCandidatesId",
                        column: x => x.MentoredCandidatesId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateMentor_User_MentorsId",
                        column: x => x.MentorsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateProcess",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    ProcessKind = table.Column<int>(type: "integer", nullable: false),
                    ProcessStateMachine = table.Column<int[]>(type: "integer[]", nullable: true),
                    CurrentStep = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateProcess", x => new { x.CandidateId, x.ProcessKind });
                    table.ForeignKey(
                        name: "FK_CandidateProcess_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    ProcessKind = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingAccessRequest",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestOutcome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingAccessRequest", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_OnboardingAccessRequest_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionName", "Description" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreate", "Генерация токена для создания кандидата" },
                    { "AccountCreationTokenUpdate", "Редактирование токена для создание кандидата" },
                    { "AccountCreationTokenView", "Просмотр токенов для создания кандидатов" },
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
                    { "DepartmentCreate", "Создание отдела" },
                    { "DepartmentDelete", "Удаление отдела" },
                    { "DepartmentUpdate", "Редактирование отдела" },
                    { "DepartmentView", "Просмотр отделов" },
                    { "FeedbackCreate", "Добавление отзыва" },
                    { "FeedbackDelete", "Удаление отзыва" },
                    { "FeedbackView", "Просмотр отзывов" },
                    { "OnboardingAccessRequestConfirm", "Подтверждение заявки на доступ к онбордингу" },
                    { "OnboardingAccessRequestCreate", "Создание заявки на доступ к онбордингу" },
                    { "OnboardingAccessRequestReject", "Отклонение заявки на доступ К онбордингу" },
                    { "OnboardingAccessRequestUpdate", "Обновление данных заявки на доступ к онбординге" },
                    { "OnboardingAccessRequestView", "Просмотр заявок на доступ к онбордингу" },
                    { "TeamCreate", "Создание команды" },
                    { "TeamView", "Просмотр команд" },
                    { "UserCreateHR", "Создание HR-пользователя" },
                    { "UserCreateMentor", "Создание наставника" },
                    { "UserLogin", "Вход пользователя" },
                    { "UserRefreshToken", "Обновление токена доступа" },
                    { "UserRegister", "Регистрация пользователя" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                column: "RoleName",
                values: new object[]
                {
                    "Admin",
                    "HR",
                    "Mentor",
                    "SuperHR"
                });

            migrationBuilder.InsertData(
                table: "PermissionRole",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "AccountCreationTokenCreate", "Admin" },
                    { "AccountCreationTokenCreate", "HR" },
                    { "AccountCreationTokenCreate", "SuperHR" },
                    { "AccountCreationTokenUpdate", "Admin" },
                    { "AccountCreationTokenUpdate", "HR" },
                    { "AccountCreationTokenUpdate", "SuperHR" },
                    { "AccountCreationTokenView", "Admin" },
                    { "AccountCreationTokenView", "HR" },
                    { "AccountCreationTokenView", "SuperHR" },
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
                    { "DepartmentCreate", "Admin" },
                    { "DepartmentCreate", "SuperHR" },
                    { "DepartmentDelete", "Admin" },
                    { "DepartmentDelete", "SuperHR" },
                    { "DepartmentUpdate", "Admin" },
                    { "DepartmentUpdate", "SuperHR" },
                    { "DepartmentView", "Admin" },
                    { "DepartmentView", "HR" },
                    { "DepartmentView", "SuperHR" },
                    { "FeedbackCreate", "Admin" },
                    { "FeedbackCreate", "SuperHR" },
                    { "FeedbackDelete", "Admin" },
                    { "FeedbackDelete", "SuperHR" },
                    { "FeedbackView", "Admin" },
                    { "FeedbackView", "HR" },
                    { "FeedbackView", "SuperHR" },
                    { "OnboardingAccessRequestConfirm", "Admin" },
                    { "OnboardingAccessRequestConfirm", "HR" },
                    { "OnboardingAccessRequestConfirm", "SuperHR" },
                    { "OnboardingAccessRequestCreate", "Admin" },
                    { "OnboardingAccessRequestCreate", "HR" },
                    { "OnboardingAccessRequestCreate", "SuperHR" },
                    { "OnboardingAccessRequestReject", "Admin" },
                    { "OnboardingAccessRequestReject", "HR" },
                    { "OnboardingAccessRequestReject", "SuperHR" },
                    { "OnboardingAccessRequestUpdate", "Admin" },
                    { "OnboardingAccessRequestUpdate", "HR" },
                    { "OnboardingAccessRequestUpdate", "SuperHR" },
                    { "OnboardingAccessRequestView", "Admin" },
                    { "OnboardingAccessRequestView", "HR" },
                    { "OnboardingAccessRequestView", "SuperHR" },
                    { "TeamCreate", "Admin" },
                    { "TeamCreate", "SuperHR" },
                    { "TeamView", "Admin" },
                    { "TeamView", "HR" },
                    { "TeamView", "SuperHR" },
                    { "UserCreateHR", "Admin" },
                    { "UserCreateHR", "SuperHR" },
                    { "UserCreateMentor", "Admin" },
                    { "UserCreateMentor", "HR" },
                    { "UserCreateMentor", "SuperHR" },
                    { "UserLogin", "Admin" },
                    { "UserLogin", "HR" },
                    { "UserLogin", "SuperHR" },
                    { "UserRefreshToken", "Admin" },
                    { "UserRefreshToken", "HR" },
                    { "UserRefreshToken", "SuperHR" },
                    { "UserRegister", "Admin" },
                    { "UserRegister", "SuperHR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCreationToken_CandidateId",
                table: "AccountCreationToken",
                column: "CandidateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountCreationToken_CreatorId",
                table: "AccountCreationToken",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_TeamId",
                table: "Candidate",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateContactInfo_CityId",
                table: "CandidateContactInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateHR_HRsId",
                table: "CandidateHR",
                column: "HRsId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateMentor_MentorsId",
                table: "CandidateMentor",
                column: "MentorsId");

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId",
                table: "City",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_CandidateId",
                table: "Feedback",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleName",
                table: "PermissionRole",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_DepartmentId",
                table: "Team",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeaderId",
                table: "Team",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_TeamsId",
                table: "TeamUser",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Login",
                table: "User",
                column: "Login",
                unique: true,
                filter: "\"Login\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCreationToken");

            migrationBuilder.DropTable(
                name: "CandidateContactInfo");

            migrationBuilder.DropTable(
                name: "CandidateHR");

            migrationBuilder.DropTable(
                name: "CandidateMentor");

            migrationBuilder.DropTable(
                name: "CandidateProcess");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "HRInfo");

            migrationBuilder.DropTable(
                name: "OnboardingAccessRequest");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "TeamUser");

            migrationBuilder.DropTable(
                name: "UserAuthToken");

            migrationBuilder.DropTable(
                name: "UserContactInfo");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
