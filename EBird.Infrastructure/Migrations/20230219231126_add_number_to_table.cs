using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    public partial class add_number_to_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "No",
                table: "Requests",
                newName: "Number");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Bird",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Bird");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Requests",
                newName: "No");
        }
    }
}
