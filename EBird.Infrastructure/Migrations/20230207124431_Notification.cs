using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class Notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_AccountId",
                table: "Notification",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Account_AccountId",
                table: "Notification",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Account_AccountId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_AccountId",
                table: "Notification");
        }
    }
}
