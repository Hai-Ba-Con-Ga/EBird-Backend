using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class account_bird_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Bird",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bird_OwnerId",
                table: "Bird",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bird_Account_OwnerId",
                table: "Bird",
                column: "OwnerId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bird_Account_OwnerId",
                table: "Bird");

            migrationBuilder.DropIndex(
                name: "IX_Bird_OwnerId",
                table: "Bird");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Bird");
        }
    }
}
