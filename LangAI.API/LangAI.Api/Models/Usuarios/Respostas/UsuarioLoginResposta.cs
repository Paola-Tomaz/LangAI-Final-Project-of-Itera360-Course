using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.API.Models.Usuarios.Respostas
{
    public class UsuarioLoginResposta
    {
        public string Token { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public int UsuarioID { get; set; }
        public int TipoUsuario { get; set; }
    }
}