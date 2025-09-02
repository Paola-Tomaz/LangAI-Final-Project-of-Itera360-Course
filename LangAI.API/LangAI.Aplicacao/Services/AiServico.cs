using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LangAI.Aplicacao.Interfaces;

namespace LangAI.Aplicacao.Services
{
    public class AiServico : IAiServico
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _model;

        public AiServico(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["Groq:ApiKey"];
            _baseUrl = configuration["Groq:BaseUrl"];
            _model = configuration["Groq:Model"];
        }

        public async Task<string> GetAiResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "Você é uma IA educacional multilíngue que gera perguntas simples de múltipla escolha para alunos iniciantes em idiomas como inglês, francês, alemão e japonês. Sempre gere uma única pergunta por vez no formato JSON. Nunca inclua explicações, apenas o JSON."
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/chat/completions")
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro na IA: {response.StatusCode} - {responseBody}");

            using var doc = JsonDocument.Parse(responseBody);
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            return result;
        }
    }
}
