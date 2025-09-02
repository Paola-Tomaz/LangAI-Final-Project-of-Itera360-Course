using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.Api.Models.Usuarios.Respostas
{
    public class UsuarioResumoResposta
    {
        public int UsuarioID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int XP { get; set; }
        public string ImagemPerfilUrl { get; set; } = string.Empty;
    }
}