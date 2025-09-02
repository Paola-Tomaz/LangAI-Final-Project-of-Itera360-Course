namespace LangAI.API.Models.Usuarios.Requisicoes
{
    public class UsuarioCriarRequisicao
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImagemPerfilUrl { get; set; } = string.Empty;
        public string IdiomaSelecionadoCodigo { get; set; } = string.Empty;
        public int TipoUsuario { get; set; }
        public string Senha { get; set; }
    }
}
