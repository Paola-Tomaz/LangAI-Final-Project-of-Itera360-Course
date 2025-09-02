namespace LangAI.Aplicacao.DTOs
{
    public class IdiomaRespostaDTO
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        public List<PersonagemRespostaDTO> Personagens { get; set; } = new();
    }
}
