using LangAI.Dominio.Entidades;
using LangAI.Repositorio.Contextos;
using Microsoft.EntityFrameworkCore;

namespace LangAI.Repositorio.Interfaces
{
    public class PersonagemRepositorio : IPersonagemRepositorio
    {
        private readonly LangAIContexto _contexto;

        public PersonagemRepositorio(LangAIContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Personagem> ObterPorIdAsync(int PersonagemID)
        {
            return await _contexto.Personagens
                .Include(p => p.Idioma)
                .FirstOrDefaultAsync(p => p.PersonagemID == PersonagemID);
        }

        public async Task<IEnumerable<Personagem>> ListarTodosAsync()
        {
            return await _contexto.Personagens
                .Include(p => p.Idioma)
                .ToListAsync();
        }

        public async Task<IEnumerable<Personagem>> ListarPorIdiomaAsync(int idiomaID)
        {
            return await _contexto.Personagens
                .Where(p => p.IdiomaID == idiomaID)
                .Include(p => p.Idioma)
                .ToListAsync();
        }

    }
}
