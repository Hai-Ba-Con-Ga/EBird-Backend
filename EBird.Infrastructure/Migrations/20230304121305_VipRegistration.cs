using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class VipRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Account_AccountId",
                table: "Payment");

            migrationBuilder.CreateTable(
                name: "VipRegistration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VipRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VipRegistration_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VipRegistration_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VipRegistration_AccountId",
                table: "VipRegistration",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_VipRegistration_PaymentId",
                table: "VipRegistration",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Account_AccountId",
                table: "Payment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Account_AccountId",
                table: "Payment");

            migrationBuilder.DropTable(
                name: "VipRegistration");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Account_AccountId",
                table: "Payment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
