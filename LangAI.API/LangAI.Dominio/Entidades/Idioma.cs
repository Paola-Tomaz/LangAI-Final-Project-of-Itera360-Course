namespace LangAI.Dominio.Entidades;

    public class Idioma
    {
        public string Codigo { get; set; } 
        public string Nome { get; set; }

        public ICollection<Personagem>? Personagens { get; set; }
    }
