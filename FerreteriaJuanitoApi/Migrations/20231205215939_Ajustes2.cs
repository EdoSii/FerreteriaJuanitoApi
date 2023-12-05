using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreteriaJuanitoApi.Migrations
{
    public partial class Ajustes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Users");
        }
    }
}
