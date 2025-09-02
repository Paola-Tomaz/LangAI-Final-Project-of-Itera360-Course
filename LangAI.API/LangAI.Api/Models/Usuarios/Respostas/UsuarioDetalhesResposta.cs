namespace LangAI.API.Models.Usuarios.Respostas
{
    public class UsuarioDetalhesResposta
    {
        public int UsuarioID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImagemPerfilUrl { get; set; } = string.Empty;
        public string IdiomaSelecionadoCodigo { get; set; } = string.Empty;
        public int TipoUsuario { get; set; }
        public int XP { get; set; }
    }
}
