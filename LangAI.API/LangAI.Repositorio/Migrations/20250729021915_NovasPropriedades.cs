using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangAI.Repositorio.Migrations
{
    public partial class NovasPropriedades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XPAlemao",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPFrances",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPIngles",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPJapones",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XPAlemao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "XPFrances",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "XPIngles",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "XPJapones",
                table: "Usuarios");
        }
    }
}
