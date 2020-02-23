using Microsoft.EntityFrameworkCore.Migrations;

namespace Moment32.Migrations
{
    public partial class AddedBorrowedCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "borrowed",
                table: "Cd",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "borrowed",
                table: "Cd");
        }
    }
}
