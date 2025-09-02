using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangAI.Repositorio.Migrations
{
    public partial class AdicionarColunaXP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "XP",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XP",
                table: "Usuarios");
        }
    }
}
