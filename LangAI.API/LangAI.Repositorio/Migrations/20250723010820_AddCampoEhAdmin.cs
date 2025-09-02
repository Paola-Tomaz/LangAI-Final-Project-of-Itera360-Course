using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangAI.Repositorio.Migrations
{
    public partial class AddCampoEhAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EhAdmin",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EhAdmin",
                table: "Usuarios");
        }
    }
}
