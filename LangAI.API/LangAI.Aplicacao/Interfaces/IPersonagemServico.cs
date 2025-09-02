using LangAI.Aplicacao.DTOs;

namespace LangAI.Aplicacao.Interfaces
{
    public interface IPersonagemServico
    {
        /// <summary>
        /// Lista todos os personagens com informações do idioma vinculado.
        /// <summary>
        Task<IEnumerable<PersonagemComIdiomaDTO>> ListarComIdiomaAsync();
    }
}