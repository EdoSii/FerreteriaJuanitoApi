using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FerreteriaJuanitoApi.Migrations
{
    public partial class Ajustes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Users_CLienteId",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "CLienteId",
                table: "Compras",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_CLienteId",
                table: "Compras",
                newName: "IX_Compras_ClienteId");

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Email);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Users_ClienteId",
                table: "Compras",
                column: "ClienteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Users_ClienteId",
                table: "Compras");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Compras",
                newName: "CLienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_ClienteId",
                table: "Compras",
                newName: "IX_Compras_CLienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Users_CLienteId",
                table: "Compras",
                column: "CLienteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
