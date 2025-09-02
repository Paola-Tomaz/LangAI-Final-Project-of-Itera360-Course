using LangAI.Aplicacao.DTOs;

namespace LangAI.Aplicacao.Interfaces
{
    public interface IIdiomaServico
    {
    /// <summary
    /// Lista todos os idiomas com seus respectivos personagens.
    /// <summary>
    Task<IEnumerable<IdiomaRespostaDTO>> ListarAsync();
    }
}