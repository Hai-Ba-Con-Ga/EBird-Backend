using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class Resoucefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountResource_Account_AccountId",
                table: "AccountResource");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountResource_Resource_ResourceId",
                table: "AccountResource");

            migrationBuilder.DropForeignKey(
                name: "FK_BirdResource_Bird_BirdId",
                table: "BirdResource");

            migrationBuilder.DropForeignKey(
                name: "FK_BirdResource_Resource_ResourceId",
                table: "BirdResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirdResource",
                table: "BirdResource");

            migrationBuilder.DropIndex(
                name: "IX_BirdResource_ResourceId",
                table: "BirdResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountResource",
                table: "AccountResource");

            migrationBuilder.RenameTable(
                name: "BirdResource",
                newName: "BirdResources");

            migrationBuilder.RenameTable(
                name: "AccountResource",
                newName: "AccountResources");

            migrationBuilder.RenameIndex(
                name: "IX_BirdResource_BirdId",
                table: "BirdResources",
                newName: "IX_BirdResources_BirdId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountResource_ResourceId",
                table: "AccountResources",
                newName: "IX_AccountResources_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountResource_AccountId",
                table: "AccountResources",
                newName: "IX_AccountResources_AccountId");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Resource",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ResourcesId",
                table: "BirdResources",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirdResources",
                table: "BirdResources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountResources",
                table: "AccountResources",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_AccountId",
                table: "Resource",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdResources_ResourcesId",
                table: "BirdResources",
                column: "ResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountResources_Account_AccountId",
                table: "AccountResources",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountResources_Resource_ResourceId",
                table: "AccountResources",
                column: "ResourceId",
                principalTable: "Resource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BirdResources_Bird_BirdId",
                table: "BirdResources",
                column: "BirdId",
                principalTable: "Bird",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BirdResources_Resource_ResourcesId",
                table: "BirdResources",
                column: "ResourcesId",
                principalTable: "Resource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Account_AccountId",
                table: "Resource",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountResources_Account_AccountId",
                table: "AccountResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountResources_Resource_ResourceId",
                table: "AccountResources");

            migrationBuilder.DropForeignKey(
                name: "FK_BirdResources_Bird_BirdId",
                table: "BirdResources");

            migrationBuilder.DropForeignKey(
                name: "FK_BirdResources_Resource_ResourcesId",
                table: "BirdResources");

            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Account_AccountId",
                table: "Resource");

            migrationBuilder.DropIndex(
                name: "IX_Resource_AccountId",
                table: "Resource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BirdResources",
                table: "BirdResources");

            migrationBuilder.DropIndex(
                name: "IX_BirdResources_ResourcesId",
                table: "BirdResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountResources",
                table: "AccountResources");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "ResourcesId",
                table: "BirdResources");

            migrationBuilder.RenameTable(
                name: "BirdResources",
                newName: "BirdResource");

            migrationBuilder.RenameTable(
                name: "AccountResources",
                newName: "AccountResource");

            migrationBuilder.RenameIndex(
                name: "IX_BirdResources_BirdId",
                table: "BirdResource",
                newName: "IX_BirdResource_BirdId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountResources_ResourceId",
                table: "AccountResource",
                newName: "IX_AccountResource_ResourceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountResources_AccountId",
                table: "AccountResource",
                newName: "IX_AccountResource_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BirdResource",
                table: "BirdResource",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountResource",
                table: "AccountResource",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BirdResource_ResourceId",
                table: "BirdResource",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountResource_Account_AccountId",
                table: "AccountResource",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountResource_Resource_ResourceId",
                table: "AccountResource",
                column: "ResourceId",
                principalTable: "Resource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BirdResource_Bird_BirdId",
                table: "BirdResource",
                column: "BirdId",
                principalTable: "Bird",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BirdResource_Resource_ResourceId",
                table: "BirdResource",
                column: "ResourceId",
                principalTable: "Resource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
