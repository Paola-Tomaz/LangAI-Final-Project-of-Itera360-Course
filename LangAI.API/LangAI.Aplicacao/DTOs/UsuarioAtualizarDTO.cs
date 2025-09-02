using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.Aplicacao.DTOs
{
    public class UsuarioAtualizarDTO
    {
        public int UsuarioID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Endereco { get; set; }
        public string? Descricao { get; set; }
    }
}