using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.Aplicacao.DTOs
{
    public class AtualizarImagemPerfilDTO
    {
        public int UsuarioID { get; set; }
        public string ImagemPerfilUrl { get; set; }
    }
}