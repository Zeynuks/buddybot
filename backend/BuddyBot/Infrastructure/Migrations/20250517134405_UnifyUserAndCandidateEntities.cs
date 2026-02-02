using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnifyUserAndCandidateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_Candidate_CandidateId",
                table: "AccountCreationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_UserId",
                table: "AccountCreationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateHR_Candidate_HRCandidatesId",
                table: "CandidateHR");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateMentor_Candidate_MentoredCandidatesId",
                table: "CandidateMentor");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateProcess_Candidate_CandidateId",
                table: "CandidateProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Candidate_CandidateId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingAccessRequest_Candidate_CandidateId",
                table: "OnboardingAccessRequest");

            migrationBuilder.DropTable(
                name: "CandidateContactInfo");

            migrationBuilder.DropTable(
                name: "TeamUser");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropIndex(
                name: "IX_AccountCreationToken_UserId",
                table: "AccountCreationToken");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AccountCreationToken");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "UserContactInfo",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OnboardingAccessTimeUtc",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "User",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                column: "RoleName",
                value: "Candidate");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactInfo_CityId",
                table: "UserContactInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TeamId",
                table: "User",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_CandidateId",
                table: "AccountCreationToken",
                column: "CandidateId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateHR_User_HRCandidatesId",
                table: "CandidateHR",
                column: "HRCandidatesId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateMentor_User_MentoredCandidatesId",
                table: "CandidateMentor",
                column: "MentoredCandidatesId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateProcess_User_CandidateId",
                table: "CandidateProcess",
                column: "CandidateId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_User_CandidateId",
                table: "Feedback",
                column: "CandidateId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingAccessRequest_User_CandidateId",
                table: "OnboardingAccessRequest",
                column: "CandidateId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Team_TeamId",
                table: "User",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContactInfo_City_CityId",
                table: "UserContactInfo",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCreationToken_User_CandidateId",
                table: "AccountCreationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateHR_User_HRCandidatesId",
                table: "CandidateHR");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateMentor_User_MentoredCandidatesId",
                table: "CandidateMentor");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateProcess_User_CandidateId",
                table: "CandidateProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_User_CandidateId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_OnboardingAccessRequest_User_CandidateId",
                table: "OnboardingAccessRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Team_TeamId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContactInfo_City_CityId",
                table: "UserContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserContactInfo_CityId",
                table: "UserContactInfo");

            migrationBuilder.DropIndex(
                name: "IX_User_TeamId",
                table: "User");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "RoleName",
                keyValue: "Candidate");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "UserContactInfo");

            migrationBuilder.DropColumn(
                name: "OnboardingAccessTimeUtc",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AccountCreationToken",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeamId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OnboardingAccessTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "CandidateContactInfo",
                columns: table => new
                {
                    CandidateId = table.Column<int>(type: "integer", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TelegramId = table.Column<long>(type: "bigint", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AccountCreationToken_UserId",
                table: "AccountCreationToken",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_TeamId",
                table: "Candidate",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateContactInfo_CityId",
                table: "CandidateContactInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUser_TeamsId",
                table: "TeamUser",
                column: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_Candidate_CandidateId",
                table: "AccountCreationToken",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCreationToken_User_UserId",
                table: "AccountCreationToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateHR_Candidate_HRCandidatesId",
                table: "CandidateHR",
                column: "HRCandidatesId",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateMentor_Candidate_MentoredCandidatesId",
                table: "CandidateMentor",
                column: "MentoredCandidatesId",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateProcess_Candidate_CandidateId",
                table: "CandidateProcess",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Candidate_CandidateId",
                table: "Feedback",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OnboardingAccessRequest_Candidate_CandidateId",
                table: "OnboardingAccessRequest",
                column: "CandidateId",
                principalTable: "Candidate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
