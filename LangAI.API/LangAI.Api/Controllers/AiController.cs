using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LangAI.Aplicacao.Interfaces;
using LangAI.Aplicacao.DTOs;
using System.Text.Json;

namespace LangAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiServico _aiServico;
        private readonly ILogger<AiController> _logger;
        private static readonly Random _random = new Random();

        private static readonly string[] _topics = {
            "Cumprimentos", "Números", "Cores", "Verbos básicos",
            "Membros da família", "Comidas e bebidas", "Animais",
            "Profissões", "Partes do corpo", "Objetos do cotidiano"
        };

        public AiController(IAiServico aiServico, ILogger<AiController> logger)
        {
            _aiServico = aiServico;
            _logger = logger;
        }

        [HttpPost("gerar")]
        public async Task<IActionResult> GerarPergunta([FromQuery] string idioma = "ingles")
        {
            try
            {
                var randomTopic = _topics[_random.Next(_topics.Length)];

                string prompt = idioma.ToLower() switch
                {
                    "ingles" => @$"
Gere uma pergunta de múltipla escolha para iniciantes.

O enunciado da pergunta deve estar em português e as 4 opções em inglês.

Formato JSON:
{{
  ""pergunta"": ""Qual é a tradução de 'dog'?"",
  ""opcoes"": [""Cachorro"", ""Gato"", ""Pássaro"", ""Peixe""],
  ""respostaCorreta"": ""Cachorro""
}}

Apenas retorne o JSON. Nenhuma explicação.",

                    "alemao" => @$"
Gere uma pergunta de múltipla escolha para iniciantes. Faça perguntas diferentes.

O enunciado da pergunta deve estar em português e as 4 opções em alemão.

Formato JSON:
{{
  ""pergunta"": ""Qual é a tradução de 'dog'?"",
  ""opcoes"": [""Hund"", ""Katze"", ""Vogel"", ""Fisch""],
  ""respostaCorreta"": ""Hund""
}}

Apenas retorne o JSON. Nenhuma explicação.",

                    "frances" => @$"
Gere uma pergunta de múltipla escolha para iniciantes. Faça perguntas diferentes.

O enunciado da pergunta deve estar em português e as 4 opções em francês.

Formato JSON:
{{
  ""pergunta"": ""Qual é a tradução de 'dog'?"",
  ""opcoes"": [""Chien"", ""Chat"", ""Oiseau"", ""Poisson""],
  ""respostaCorreta"": ""Chien""
}}

Apenas retorne o JSON. Nenhuma explicação.",

                    "japones" => @$"
Gere uma pergunta de múltipla escolha para iniciantes. Faça perguntas diferentes.

O enunciado da pergunta deve estar em português e as 4 opções em japonês (use a escrita em hiragana, katakana ou kanji).

Formato JSON:
{{
  ""pergunta"": ""Qual é a tradução de 'dog'?"",
  ""opcoes"": [""いぬ"", ""ねこ"", ""とり"", ""さかな""],
  ""respostaCorreta"": ""いぬ""
}}

Apenas retorne o JSON. Nenhuma explicação.",

                    _ => @$"
Gere uma pergunta de múltipla escolha para iniciantes. Faça perguntas diferentes.

O enunciado da pergunta deve estar em português e as 4 opções em inglês.

Formato JSON:
{{
  ""pergunta"": ""Qual é a tradução de 'dog'?"",
  ""opcoes"": [""Cachorro"", ""Gato"", ""Pássaro"", ""Peixe""],
  ""respostaCorreta"": ""Cachorro""
}}

Apenas retorne o JSON. Nenhuma explicação."
                };

                _logger.LogInformation("Prompt enviado para IA:\n{prompt}", prompt);

                var respostaRaw = await _aiServico.GetAiResponseAsync(prompt);
                _logger.LogInformation("Resposta crua:\n{respostaRaw}", respostaRaw);

                var textoIA = ExtractJsonFromText(respostaRaw);
                _logger.LogInformation("Texto limpo:\n{textoIA}", textoIA);

                var pergunta = JsonSerializer.Deserialize<PerguntaDTO>(textoIA);
                if (IsValidQuestion(pergunta))
                {
                    _logger.LogInformation("Pergunta válida recebida.");
                    return Ok(new { sucesso = true, pergunta });
                }

                _logger.LogWarning("Pergunta inválida: {textoIA}", textoIA);
                return Ok(new { sucesso = false, pergunta = GetRandomDefaultQuestion(idioma) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar pergunta com IA.");
                return Ok(new { sucesso = false, pergunta = GetRandomDefaultQuestion(idioma) });
            }
        }

        private string ExtractJsonFromText(string input)
        {
            int start = input.IndexOf('{');
            int end = input.LastIndexOf('}');
            if (start >= 0 && end > start)
                return input.Substring(start, end - start + 1).Trim();

            return input.Trim();
        }

        private bool IsValidQuestion(PerguntaDTO question)
        {
            if (question == null) return false;
            if (string.IsNullOrWhiteSpace(question.pergunta)) return false;
            if (question.opcoes == null || question.opcoes.Count != 4) return false;
            if (string.IsNullOrWhiteSpace(question.respostaCorreta)) return false;
            if (!question.opcoes.Contains(question.respostaCorreta)) return false;
            if (question.opcoes.Distinct().Count() != 4) return false;
            return true;
        }

        private PerguntaDTO GetRandomDefaultQuestion(string idioma)
        {
            var defaultQuestionsIngles = new List<PerguntaDTO>
            {
                new PerguntaDTO {
                    pergunta = "Como se diz 'Bom dia' em inglês?",
                    opcoes = new List<string> { "Good night", "Good morning", "Good afternoon", "Hello" },
                    respostaCorreta = "Good morning"
                },
                new PerguntaDTO {
                    pergunta = "Qual é a tradução de 'apple'?",
                    opcoes = new List<string> { "Banana", "Maçã", "Laranja", "Uva" },
                    respostaCorreta = "Maçã"
                }
            };

            var defaultQuestionsAlemao = new List<PerguntaDTO>
            {
                new PerguntaDTO {
                    pergunta = "Como se diz 'Bom dia' em alemão?",
                    opcoes = new List<string> { "Guten Abend", "Guten Tag", "Gute Nacht", "Hallo" },
                    respostaCorreta = "Guten Tag"
                }
            };

            var defaultQuestionsFrances = new List<PerguntaDTO>
            {
                new PerguntaDTO {
                    pergunta = "Como se diz 'Bom dia' em francês?",
                    opcoes = new List<string> { "Bonsoir", "Bonjour", "Bonne nuit", "Salut" },
                    respostaCorreta = "Bonjour"
                }
            };

            var defaultQuestionsJapones = new List<PerguntaDTO>
            {
                new PerguntaDTO {
                    pergunta = "Como se diz 'Bom dia' em japonês?",
                    opcoes = new List<string> { "こんばんは", "おはようございます", "おやすみなさい", "こんにちは" },
                    respostaCorreta = "おはようございます"
                }
            };

            return idioma.ToLower() switch
            {
                "alemao" => defaultQuestionsAlemao[_random.Next(defaultQuestionsAlemao.Count)],
                "frances" => defaultQuestionsFrances[_random.Next(defaultQuestionsFrances.Count)],
                "japones" => defaultQuestionsJapones[_random.Next(defaultQuestionsJapones.Count)],
                _ => defaultQuestionsIngles[_random.Next(defaultQuestionsIngles.Count)],
            };
        }
    }
}
