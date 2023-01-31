using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class update_name_GroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupEntity_Account_CreatedById",
                table: "GroupEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupEntity",
                table: "GroupEntity");

            migrationBuilder.RenameTable(
                name: "GroupEntity",
                newName: "Group");

            migrationBuilder.RenameIndex(
                name: "IX_GroupEntity_CreatedById",
                table: "Group",
                newName: "IX_Group_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Account_CreatedById",
                table: "Group",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Account_CreatedById",
                table: "Group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "GroupEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Group_CreatedById",
                table: "GroupEntity",
                newName: "IX_GroupEntity_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupEntity",
                table: "GroupEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupEntity_Account_CreatedById",
                table: "GroupEntity",
                column: "CreatedById",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
