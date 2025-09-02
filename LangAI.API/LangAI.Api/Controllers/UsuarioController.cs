using LangAI.API.Models.Usuarios.Requisicoes;
using LangAI.API.Models.Usuarios.Respostas;
using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using LangAI.Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;

namespace LangAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioServico _usuarioServico;

        public UsuariosController(IConfiguration configuration, IUsuarioServico usuarioServico)
        {
            _configuration = configuration;
            _usuarioServico = usuarioServico;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLogarRequisicao login)
        {
            var user = await _usuarioServico.ValidateUser(login.Email, login.Senha);
            if (user == null)
                return Unauthorized("Usuário ou senha inválidos");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Nome),
                new Claim("userId", user.UsuarioID.ToString()),
                new Claim("tipoUsuario", ((int)user.TipoUsuario).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Emissor"],
                audience: _configuration["Jwt:Publico"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return Ok(new UsuarioLoginResposta
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Nome = user.Nome,
                UsuarioID = user.UsuarioID,
                TipoUsuario = (int)user.TipoUsuario
            });
        }

        [HttpPost("Criar")]
        public async Task<IActionResult> Criar([FromBody] UsuarioCriarRequisicao requisicao)
        {
            var usuarioDto = new UsuarioDTO
            {
                Nome = requisicao.Nome,
                Email = requisicao.Email,
                IdiomaSelecionadoCodigo = requisicao.IdiomaSelecionadoCodigo,
                TipoUsuario = TipoUsuarioEnum.Aluno,
                Senha = requisicao.Senha
            };

            await _usuarioServico.CriarAsync(usuarioDto);
            return Ok("Usuário criado com sucesso!");
        }

        [HttpPut("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO dto)
        {
            await _usuarioServico.AlterarSenhaAsync(dto);
            return Ok(new { mensagem = "Senha alterada com sucesso!" });
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] UsuarioAtualizarRequisicao requisicao)
        {
            try
            {
                var usuarioExistente = await _usuarioServico.ObterPorIdAsync(requisicao.UsuarioID);
                if (usuarioExistente == null)
                    return NotFound("Usuário não encontrado.");

                var dtoAtualizado = new UsuarioAtualizarDTO
                {
                    UsuarioID = requisicao.UsuarioID,
                    Nome = requisicao.Nome ?? usuarioExistente.Nome,
                    Email = requisicao.Email ?? usuarioExistente.Email,
                    Telefone = requisicao.Telefone ?? usuarioExistente.Telefone,
                    Endereco = requisicao.Endereco ?? usuarioExistente.Endereco,
                    Descricao = requisicao.Descricao ?? usuarioExistente.Descricao
                };

                await _usuarioServico.AtualizarAsync(dtoAtualizado);
                return Ok("Perfil atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ObterPorId/{usuarioId}")]
        public async Task<ActionResult<UsuarioDetalhesResposta>> ObterPorId(int usuarioId)
        {
            var dto = await _usuarioServico.ObterPorIdAsync(usuarioId);
            if (dto == null) return NotFound();

            return Ok(new UsuarioDetalhesResposta
            {
                UsuarioID = dto.UsuarioID,
                Nome = dto.Nome,
                Email = dto.Email,
                ImagemPerfilUrl = dto.ImagemPerfilUrl,
                IdiomaSelecionadoCodigo = dto.IdiomaSelecionadoCodigo,
                TipoUsuario = (int)dto.TipoUsuario,
                XP = dto.XP
            });
        }

        [HttpGet("ListarTodos")]
        public async Task<ActionResult<IEnumerable<UsuarioDetalhesResposta>>> Listar()
        {
            var usuarios = await _usuarioServico.ListarTodosAsync();
            if (usuarios == null || !usuarios.Any())
                return NotFound("Nenhum usuário encontrado.");

            var response = usuarios.Select(dto => new UsuarioDetalhesResposta
            {
                UsuarioID = dto.UsuarioID,
                Nome = dto.Nome,
                Email = dto.Email,
                ImagemPerfilUrl = dto.ImagemPerfilUrl,
                IdiomaSelecionadoCodigo = dto.IdiomaSelecionadoCodigo,
                TipoUsuario = (int)dto.TipoUsuario,
                XP = dto.XP
            }).ToList();

            return Ok(response);
        }

        [HttpPut("atualizar-foto")]
        public async Task<IActionResult> AtualizarImagemPerfilAsync([FromBody] AtualizarImagemPerfilDTO dto)
        {
            if (dto.UsuarioID <= 0)
                return BadRequest("ID do usuário é obrigatório.");

            await _usuarioServico.AtualizarImagemPerfilAsync(dto.UsuarioID, dto.ImagemPerfilUrl);

            return Ok(new { mensagem = "Imagem de perfil atualizada com sucesso.", imagemUrl = dto.ImagemPerfilUrl });
        }


        [HttpPost("atualizarXP")]
        [Authorize]
        public async Task<IActionResult> AtualizarXP([FromBody] AtualizarXPDTO dto)
        {
            var userId = int.Parse(User.FindFirst("userId").Value);
            var atualizado = await _usuarioServico.AtualizarXPAsync(userId, dto.XP);

            if (!atualizado)
                return BadRequest("Falha ao atualizar XP");

            var usuario = await _usuarioServico.ObterPorIdAsync(userId);
            return Ok(new { XP = usuario.XP });
        }

        [HttpGet("gerar-url-upload")]
        public IActionResult GerarUrlUpload([FromQuery] int usuarioId, [FromQuery] string extensao)
        {
            var accessKey = _configuration["AWS:AccessKey"];
            var secretKey = _configuration["AWS:SecretKey"];
            var region = _configuration["AWS:Region"];
            var bucket = _configuration["AWS:BucketName"];

            var nomeArquivo = $"profile-pics/{usuarioId}/{DateTime.UtcNow.Ticks}.{extensao}";
            var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
            var contentType = ObterMimeType(extensao);

            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = nomeArquivo,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(5),
                ContentType = contentType
            };

            var url = s3Client.GetPreSignedURL(request);

            return Ok(new
            {
                uploadUrl = url,
                fileUrl = $"https://{bucket}.s3.{region}.amazonaws.com/{nomeArquivo}"
            });
        }

        [HttpGet("progresso/{id}")]
        public async Task<IActionResult> ObterProgressoPorIdioma(int id)
        {
            var progresso = await _usuarioServico.ObterXpPorIdiomaAsync(id);
            return Ok(progresso);
        }



        [HttpDelete("Remover/{usuarioId}")]
        public async Task<IActionResult> Remover(int usuarioId)
        {
            try
            {
                await _usuarioServico.RemoverAsync(usuarioId);
                return Ok("Usuário removido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao deletar o usuário: {ex.Message}");
            }
        }

        private string ObterMimeType(string extensao)
        {
            return extensao.ToLower() switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "bmp" => "image/bmp",
                _ => "application/octet-stream",
            };
        }
    }
}
