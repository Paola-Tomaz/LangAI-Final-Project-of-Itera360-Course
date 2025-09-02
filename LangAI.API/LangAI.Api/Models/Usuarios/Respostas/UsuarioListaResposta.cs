namespace LangAI.API.Models.Usuarios.Respostas
{
    public class UsuarioListaResposta
    {
        public int UsuarioID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TipoUsuario { get; set; }
        public bool Ativo { get; set; }
    }
}
