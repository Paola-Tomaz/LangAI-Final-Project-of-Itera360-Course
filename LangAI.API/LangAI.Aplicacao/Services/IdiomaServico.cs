using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using Microsoft.EntityFrameworkCore;
using LangAI.Repositorio.Contextos;


namespace LangAI.Aplicacao.Services
{
    public class IdiomaServico : IIdiomaServico
    {
        private readonly LangAIContexto _contexto;

        public IdiomaServico(LangAIContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<IdiomaRespostaDTO>> ListarAsync()
        {
            var idiomas = await _contexto.Idiomas
                .Include(i => i.Personagens)
                .ToListAsync();

            return idiomas.Select(i => new IdiomaRespostaDTO
            {
                Codigo = i.Codigo,
                Nome = i.Nome,
                Personagens = i.Personagens.Select(p => new PersonagemRespostaDTO
                {
                    PersonagemID = p.PersonagemID,
                    Nome = p.Nome,
                    ImagemUrl = p.ImagemUrl,
                    FraseApresentacao = p.FraseApresentacao
                }).ToList()
            });
        }
    }
}
