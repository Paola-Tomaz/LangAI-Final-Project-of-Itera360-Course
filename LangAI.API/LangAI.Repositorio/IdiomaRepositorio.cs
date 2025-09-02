using LangAI.Dominio.Entidades;
using LangAI.Repositorio.Contextos;
using LangAI.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LangAI.Repositorio.Interfaces
{
    public class IdiomaRepositorio : IIdiomaRepositorio
    {
        private readonly LangAIContexto _contexto;

        public IdiomaRepositorio(LangAIContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Idioma>> ListarAsync()
        {
            return await _contexto.Idiomas.ToListAsync();
        }
    }
}
