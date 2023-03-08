using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class update_MatchDetailEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchBirdId",
                table: "MatchResources");

            migrationBuilder.RenameColumn(
                name: "MatchBirdId",
                table: "MatchResources",
                newName: "MatchDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResources_MatchBirdId",
                table: "MatchResources",
                newName: "IX_MatchResources_MatchDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchDetailId",
                table: "MatchResources",
                column: "MatchDetailId",
                principalTable: "MatchDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchDetailId",
                table: "MatchResources");

            migrationBuilder.RenameColumn(
                name: "MatchDetailId",
                table: "MatchResources",
                newName: "MatchBirdId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResources_MatchDetailId",
                table: "MatchResources",
                newName: "IX_MatchResources_MatchBirdId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResources_MatchDetail_MatchBirdId",
                table: "MatchResources",
                column: "MatchBirdId",
                principalTable: "MatchDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
