using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class add_isReady_to_requestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Requests",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Requests");
        }
    }
}
