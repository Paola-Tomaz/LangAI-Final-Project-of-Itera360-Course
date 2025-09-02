namespace LangAI.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ObterPorIdAsync(int usuarioId);
        Task<int> SalvarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RestaurarAsync(Usuario usuario);

        Task<Usuario?> ObterDeletadoAsync(int usuarioId);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ListarTodosAsync();
        Task<bool> AtualizarXPAsync(int usuarioId, int incrementoXP);
        Task<Dictionary<string, int>> ObterXPPorIdiomaAsync(int usuarioId);

    }
}