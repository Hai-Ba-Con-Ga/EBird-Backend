using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class add_requestReferenceToMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromRequestId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FromRequestId",
                table: "Matches",
                column: "FromRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Requests_FromRequestId",
                table: "Matches",
                column: "FromRequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Requests_FromRequestId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_FromRequestId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FromRequestId",
                table: "Matches");
        }
    }
}
