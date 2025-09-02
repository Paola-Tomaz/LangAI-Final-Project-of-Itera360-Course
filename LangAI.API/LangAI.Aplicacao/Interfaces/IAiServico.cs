namespace LangAI.Aplicacao.Interfaces
{
    public interface IAiServico
    {
        Task<string> GetAiResponseAsync(string prompt);
    }
}
