using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class update_resourceEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumnail",
                table: "Resource",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumnail",
                table: "Resource");
        }
    }
}
