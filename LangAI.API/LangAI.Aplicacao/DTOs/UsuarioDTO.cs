using LangAI.Aplicacao.DTOs;
using LangAI.Dominio.Enumeradores;
using System.ComponentModel.DataAnnotations;

namespace LangAI.Aplicacao.DTOs

{
    public class UsuarioDTO
    {

        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]

        public string? Email { get; set; }
        public string? ImagemPerfilUrl { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string? Descricao { get; set; } 
        public string? IdiomaSelecionadoCodigo { get; set; }
        public TipoUsuarioEnum TipoUsuario { get; set; }
        public bool Ativo { get; set; } = true;
        public int XP { get; set; }


        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(6, ErrorMessage = "A senha deve ter ao menos 6 caracteres")]
        public string? Senha { get; set; }
    }
}
