using LangAI.Aplicacao.DTOs;

namespace LangAI.Aplicacao.Interfaces
{
    public interface IUsuarioServico
    {
        Task<int> CriarAsync(UsuarioDTO usuarioDto);
        Task AlterarSenhaAsync(AlterarSenhaDTO dto);
        Task AtualizarAsync(UsuarioAtualizarDTO dto);
        Task RemoverAsync(int usuarioId);
        Task RestaurarAsync(int usuarioId);
        Task<IEnumerable<UsuarioDTO>> ListarTodosAsync();
        Task<string> LogarAsync(string email, string senha);
        Task<UsuarioDTO> ObterPorIdAsync(int usuarioId);
        Task<UsuarioDTO> ObterPorEmailAsync(string email);
        Task<UsuarioDTO?> ValidateUser(string email, string senha);
        Task AtualizarImagemPerfilAsync(int usuarioId, string ImagemPerfilUrl);

        Task<bool> AtualizarXPAsync(int usuarioId, int xp);
        Task<Dictionary<string, int>> ObterXpPorIdiomaAsync(int usuarioId);


    }
}
