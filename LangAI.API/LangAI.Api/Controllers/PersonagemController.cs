using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LangAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagensController : ControllerBase
    {
        private readonly IPersonagemServico _personagemServico;

        public PersonagensController(IPersonagemServico personagemServico)
        {
            _personagemServico = personagemServico;
        }

        [HttpGet]
        [Route("ListarPersonagensComIdioma")]
        public async Task<ActionResult<IEnumerable<PersonagemComIdiomaDTO>>> ListarPersonagensComIdioma()
        {
            try
            {
                var personagens = await _personagemServico.ListarComIdiomaAsync();

                if (personagens == null || !personagens.Any())
                    return NotFound("Nenhum personagem encontrado.");

                return Ok(personagens);
            }
            catch (Exception ex)
            {
                // Logue o erro aqui se tiver algum mecanismo de logging
                return StatusCode(500, $"Erro ao listar personagens: {ex.Message}");
            }
        }
    }
}
