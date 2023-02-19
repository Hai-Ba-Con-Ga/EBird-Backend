using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class UpdateMatchEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResources_MatchBirds_MatchBirdId",
                table: "MatchResources");

            migrationBuilder.DropTable(
                name: "MatchBirds");

            migrationBuilder.CreateTable(
                name: "MatchDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfterElo = table.Column<int>(type: "int", nullable: true),
                    BeforeElo = table.Column<int>(type: "int", nullable: false),
                    UpdateDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchDetail_Bird_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Bird",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchDetail_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchDetail_BirdId",
                table: "MatchDetail",
                column: "BirdId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchDetail_MatchId",
                table: "MatchDetail",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchBirdId",
                table: "MatchResources",
                column: "MatchBirdId",
                principalTable: "MatchDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchBirdId",
                table: "MatchResources");

            migrationBuilder.DropTable(
                name: "MatchDetail");

            migrationBuilder.CreateTable(
                name: "MatchBirds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AfterElo = table.Column<int>(type: "int", nullable: true),
                    BeforeElo = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDatetime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchBirds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchBirds_Bird_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Bird",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MatchBirds_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchBirds_BirdId",
                table: "MatchBirds",
                column: "BirdId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchBirds_MatchId",
                table: "MatchBirds",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResources_MatchBirds_MatchBirdId",
                table: "MatchResources",
                column: "MatchBirdId",
                principalTable: "MatchBirds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
