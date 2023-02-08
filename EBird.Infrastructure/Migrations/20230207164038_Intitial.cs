﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class Intitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "varchar", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    Username = table.Column<string>(type: "varchar", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BirdType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BirdTypeCode = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BirdTypeName = table.Column<string>(type: "nvarchar", maxLength: 100, nullable: false),
                    BirdTypeCreatedDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifcationStore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifcationStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    GroupMaxELO = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupMinELO = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupStatus = table.Column<string>(type: "varchar", maxLength: 20, nullable: false),
                    GroupCreateDatetime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedById = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Group_Account_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    JwtId = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRevoked = table.Column<bool>(type: "INTEGER", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceDataLink = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    ResourceDescription = table.Column<string>(type: "nvarchar", maxLength: 100, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ResourceCreateById = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_Account_ResourceCreateById",
                        column: x => x.ResourceCreateById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    RoomStatus = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    RoomCity = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    RoomCreateDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoomCreateById = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_Account_RoomCreateById",
                        column: x => x.RoomCreateById,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bird",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BirdName = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BirdAge = table.Column<int>(type: "int", nullable: false),
                    BirdWeight = table.Column<float>(type: "float", nullable: false),
                    BirdElo = table.Column<int>(type: "int", nullable: false),
                    BirdStatus = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BirdCreatedDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    BirdDescription = table.Column<string>(type: "text", nullable: false),
                    BirdColor = table.Column<string>(type: "nvarchar", maxLength: 50, nullable: false),
                    BirdTypeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bird", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bird_Account_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bird_BirdType_BirdTypeId",
                        column: x => x.BirdTypeId,
                        principalTable: "BirdType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account_Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceEntityId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Resource_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account_Resource_Resource_ResourceEntityId",
                        column: x => x.ResourceEntityId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bird_Resource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResourceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BirdId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bird_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bird_Resource_Bird_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Bird",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bird_Resource_Resource_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Resource_AccountId_ResourceId",
                table: "Account_Resource",
                columns: new[] { "AccountId", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Resource_ResourceEntityId",
                table: "Account_Resource",
                column: "ResourceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Bird_BirdTypeId",
                table: "Bird",
                column: "BirdTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bird_OwnerId",
                table: "Bird",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bird_Resource_BirdId_ResourceId",
                table: "Bird_Resource",
                columns: new[] { "BirdId", "ResourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_Bird_Resource_ResourceId",
                table: "Bird_Resource",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdType_BirdTypeCode",
                table: "BirdType",
                column: "BirdTypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Group_CreatedById",
                table: "Group",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AccountId",
                table: "RefreshToken",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ResourceCreateById",
                table: "Resource",
                column: "ResourceCreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Room_RoomCreateById",
                table: "Room",
                column: "RoomCreateById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Resource");

            migrationBuilder.DropTable(
                name: "Bird_Resource");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "VerifcationStore");

            migrationBuilder.DropTable(
                name: "Bird");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "BirdType");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}