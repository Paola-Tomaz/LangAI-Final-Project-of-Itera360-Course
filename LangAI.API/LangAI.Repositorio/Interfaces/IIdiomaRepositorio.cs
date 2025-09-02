using LangAI.Dominio.Entidades;

public interface IIdiomaRepositorio
{
    Task<IEnumerable<Idioma>> ListarAsync();
}