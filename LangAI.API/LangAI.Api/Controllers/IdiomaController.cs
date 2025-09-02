using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LangAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdiomaController : ControllerBase
    {
        private readonly IIdiomaServico _idiomaServico;

        public IdiomaController(IIdiomaServico idiomaServico)
        {
            _idiomaServico = idiomaServico;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdiomaRespostaDTO>>> Listar()
        {
            try
            {
                var idiomas = await _idiomaServico.ListarAsync();

                if (idiomas == null || !idiomas.Any())
                    return NotFound("Nenhum idioma encontrado.");

                return Ok(idiomas);
            }
            catch (Exception ex)
            {
                // Aqui pode logar o erro, em breve
                return StatusCode(500, $"Erro ao listar idiomas: {ex.Message}");
            }
        }
    }
}
