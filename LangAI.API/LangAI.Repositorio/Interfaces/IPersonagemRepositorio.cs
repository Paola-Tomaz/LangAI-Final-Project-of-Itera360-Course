using LangAI.Dominio.Entidades;


namespace LangAI.Repositorio.Interfaces
{
    public interface IPersonagemRepositorio
    {
        Task<Personagem> ObterPorIdAsync(int PersonagemID);
        Task<IEnumerable<Personagem>> ListarTodosAsync();
        Task<IEnumerable<Personagem>> ListarPorIdiomaAsync(int IdiomaCodigo);
    }

}