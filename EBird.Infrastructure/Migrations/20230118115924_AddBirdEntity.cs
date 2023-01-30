using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class AddBirdEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BirdTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirdTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirdTypeCreatedDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Birds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirdName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirdAge = table.Column<int>(type: "int", nullable: false),
                    BirdWeight = table.Column<double>(type: "float", nullable: false),
                    BirdElo = table.Column<int>(type: "int", nullable: false),
                    BirdStatus = table.Column<bool>(type: "bit", nullable: false),
                    BirdCreatedDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    BirdDescription = table.Column<string>(type: "text", nullable: false),
                    BirdColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirdTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Birds_BirdTypes_BirdTypeId",
                        column: x => x.BirdTypeId,
                        principalTable: "BirdTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Birds_BirdTypeId",
                table: "Birds",
                column: "BirdTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdTypes_BirdTypeCode",
                table: "BirdTypes",
                column: "BirdTypeCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Birds");

            migrationBuilder.DropTable(
                name: "BirdTypes");
        }
    }
}
