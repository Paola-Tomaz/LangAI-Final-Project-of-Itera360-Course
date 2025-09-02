namespace LangAI.API.Models.Usuarios.Requisicoes
{
    public class UsuarioAtualizarRequisicao
    {
        public int UsuarioID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string? Descricao { get; set; }
    }
}
