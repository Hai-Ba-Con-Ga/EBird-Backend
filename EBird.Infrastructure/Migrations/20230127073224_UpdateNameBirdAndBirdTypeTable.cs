using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class UpdateNameBirdAndBirdTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Birds_BirdTypes_BirdTypeId",
                table: "Birds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirdTypes",
                table: "BirdTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Birds",
                table: "Birds");

            migrationBuilder.RenameTable(
                name: "BirdTypes",
                newName: "BirdType");

            migrationBuilder.RenameTable(
                name: "Birds",
                newName: "Bird");

            migrationBuilder.RenameIndex(
                name: "IX_BirdTypes_BirdTypeCode",
                table: "BirdType",
                newName: "IX_BirdType_BirdTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Birds_BirdTypeId",
                table: "Bird",
                newName: "IX_Bird_BirdTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirdType",
                table: "BirdType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bird",
                table: "Bird",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bird_BirdType_BirdTypeId",
                table: "Bird",
                column: "BirdTypeId",
                principalTable: "BirdType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bird_BirdType_BirdTypeId",
                table: "Bird");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirdType",
                table: "BirdType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bird",
                table: "Bird");

            migrationBuilder.RenameTable(
                name: "BirdType",
                newName: "BirdTypes");

            migrationBuilder.RenameTable(
                name: "Bird",
                newName: "Birds");

            migrationBuilder.RenameIndex(
                name: "IX_BirdType_BirdTypeCode",
                table: "BirdTypes",
                newName: "IX_BirdTypes_BirdTypeCode");

            migrationBuilder.RenameIndex(
                name: "IX_Bird_BirdTypeId",
                table: "Birds",
                newName: "IX_Birds_BirdTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirdTypes",
                table: "BirdTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Birds",
                table: "Birds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Birds_BirdTypes_BirdTypeId",
                table: "Birds",
                column: "BirdTypeId",
                principalTable: "BirdTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
