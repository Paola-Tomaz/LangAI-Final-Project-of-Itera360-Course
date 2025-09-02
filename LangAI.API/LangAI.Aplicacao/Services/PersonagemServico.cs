using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using LangAI.Repositorio.Contextos;
using Microsoft.EntityFrameworkCore;

namespace LangAI.Aplicacao.Services
{
    public class PersonagemServico : IPersonagemServico
    {
        private readonly LangAIContexto _contexto;

        public PersonagemServico(LangAIContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<PersonagemComIdiomaDTO>> ListarComIdiomaAsync()
        {
            var personagens = await _contexto.Personagens
                .Include(p => p.Idioma)
                .ToListAsync();

            return personagens.Select(p => new PersonagemComIdiomaDTO
            {
                PersonagemID = p.PersonagemID,
                Nome = p.Nome,
                ImagemUrl = p.ImagemUrl,
                FraseApresentacao = p.FraseApresentacao,
                IdiomaCodigo = p.IdiomaCodigo,
                IdiomaNome = p.Idioma?.Nome ?? string.Empty
            });
        }
    }
}
