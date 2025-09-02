namespace LangAI.Aplicacao.DTOs
{
    public class PersonagemComIdiomaDTO
    {
        public int PersonagemID { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
        public string FraseApresentacao { get; set; } = string.Empty;

        public int IdiomaID { get; set; }
        public string IdiomaCodigo { get; set; } = string.Empty;
        public string IdiomaNome { get; set; } = string.Empty; 
    }
}
