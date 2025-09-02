using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LangAI.Aplicacao.DTOs
{
    public class PerguntaDTO
    {
        public string pergunta { get; set; }
        public List<string> opcoes { get; set; }
        public string respostaCorreta { get; set; }
    }
}