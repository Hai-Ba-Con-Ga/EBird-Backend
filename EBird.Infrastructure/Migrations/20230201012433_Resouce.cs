using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class Resouce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Datalink = table.Column<string>(type: "varchar", nullable: true),
                    Description = table.Column<string>(type: "varchar", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountResource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountResource_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountResource_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BirdResource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BirdResource_Bird_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Bird",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BirdResource_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountResource_AccountId",
                table: "AccountResource",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountResource_ResourceId",
                table: "AccountResource",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdResource_BirdId",
                table: "BirdResource",
                column: "BirdId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdResource_ResourceId",
                table: "BirdResource",
                column: "ResourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountResource");

            migrationBuilder.DropTable(
                name: "BirdResource");

            migrationBuilder.DropTable(
                name: "Resource");
        }
    }
}
