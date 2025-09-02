using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.Aplicacao.DTOs
{
    public class AlterarSenhaDTO
    {
        public int UsuarioID { get; set; }
        public string NovaSenha { get; set; } = string.Empty;
    }
}