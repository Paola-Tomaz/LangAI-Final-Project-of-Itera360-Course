namespace LangAI.Dominio.Entidades;

public class Personagem
{
    public int PersonagemID { get; set; }
    public string Nome { get; set; }
    public string ImagemUrl { get; set; }
    public string FraseApresentacao { get; set; }

    public int IdiomaID { get; set; }
    public string IdiomaCodigo { get; set; }
    public Idioma Idioma { get; set; }
}
