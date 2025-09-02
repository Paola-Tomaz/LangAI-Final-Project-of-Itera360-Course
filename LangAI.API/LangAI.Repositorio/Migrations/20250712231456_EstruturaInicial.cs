using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangAI.Repositorio.Migrations
{
    public partial class EstruturaInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Personagens",
                columns: table => new
                {
                    PersonagemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraseApresentacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdiomaID = table.Column<int>(type: "int", nullable: false),
                    IdiomaCodigo = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personagens", x => x.PersonagemID);
                    table.ForeignKey(
                        name: "FK_Personagens_Idiomas_IdiomaCodigo",
                        column: x => x.IdiomaCodigo,
                        principalTable: "Idiomas",
                        principalColumn: "Codigo");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    ImagemPerfilUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdiomaSelecionadoCodigo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TipoUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                    table.ForeignKey(
                        name: "FK_Usuarios_Idiomas_IdiomaSelecionadoCodigo",
                        column: x => x.IdiomaSelecionadoCodigo,
                        principalTable: "Idiomas",
                        principalColumn: "Codigo");
                });

            migrationBuilder.InsertData(
                table: "Idiomas",
                columns: new[] { "Codigo", "Nome" },
                values: new object[,]
                {
                    { "de-DE", "Alemão" },
                    { "en-US", "Inglês" },
                    { "fr-CA", "Francês" },
                    { "ja-JP", "Japonês" },
                    { "pt-BR", "Português" }
                });

            migrationBuilder.InsertData(
                table: "Personagens",
                columns: new[] { "PersonagemID", "FraseApresentacao", "IdiomaCodigo", "IdiomaID", "ImagemUrl", "Nome" },
                values: new object[,]
                {
                    { 1, "Olá, sou a Lola e vou te ensinar Português!", "pt-BR", 0, "", "Lola" },
                    { 2, "Hi, I'm Tyler and i'll teach you English!", "en-US", 0, "", "Tyler" },
                    { 3, "Bonjour, je suis Sebastian et je vais vous apprendre le français!", "fr-CA", 0, "", "Sebastian" },
                    { 4, "Hallo, ich bin Klaus und ich bringe dir Französisch bei!", "de-DE", 0, "", "Klaus" },
                    { 5, "こんにちは、I’m Miyuki と I’l が日本語をお教えします！", "ja-JP", 0, "", "Miyuki" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_IdiomaCodigo",
                table: "Personagens",
                column: "IdiomaCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdiomaSelecionadoCodigo",
                table: "Usuarios",
                column: "IdiomaSelecionadoCodigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personagens");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Idiomas");
        }
    }
}
