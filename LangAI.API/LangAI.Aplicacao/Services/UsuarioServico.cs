using LangAI.Aplicacao.DTOs;
using LangAI.Aplicacao.Interfaces;
using LangAI.Repositorio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LangAI.Aplicacao.Configuracoes;
using Microsoft.Extensions.Configuration;

namespace LangAI.Aplicacao.Services
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly JwtConfiguracoes _jwtConfiguracoes;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IConfiguration configuration)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _passwordHasher = new PasswordHasher<string>();
            _jwtConfiguracoes = configuration.GetSection("Jwt").Get<JwtConfiguracoes>();
        }



        public async Task<int> CriarAsync(UsuarioDTO usuarioDto)
        {
            var senhaCriptografada = _passwordHasher.HashPassword(usuarioDto.Email!, usuarioDto.Senha!);

            var usuario = new Usuario
            {
                Nome = usuarioDto.Nome!,
                Email = usuarioDto.Email!,
                ImagemPerfilUrl = usuarioDto.ImagemPerfilUrl ?? string.Empty,
                IdiomaSelecionadoCodigo = usuarioDto.IdiomaSelecionadoCodigo!,
                TipoUsuario = usuarioDto.TipoUsuario,
                Senha = senhaCriptografada,
                Ativo = true
            };

            return await _usuarioRepositorio.SalvarAsync(usuario);

        }

        public async Task AtualizarAsync(UsuarioAtualizarDTO dto)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(dto.UsuarioID);
            if (usuario == null || !usuario.Ativo)
                throw new Exception("Usuário não encontrado ou inativo.");

            usuario.Nome = dto.Nome ?? usuario.Nome;
            usuario.Email = dto.Email ?? usuario.Email;
            usuario.Telefone = dto.Telefone ?? usuario.Telefone;
            usuario.Endereco = dto.Endereco ?? usuario.Endereco;
            usuario.Descricao = dto.Descricao ?? usuario.Descricao;

            await _usuarioRepositorio.AtualizarAsync(usuario);
        }

        public async Task AlterarSenhaAsync(AlterarSenhaDTO dto)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(dto.UsuarioID);
            if (usuario == null || !usuario.Ativo)
                throw new Exception("Usuário não encontrado ou inativo.");

            var senhaHash = _passwordHasher.HashPassword(usuario.Email, dto.NovaSenha);
            usuario.Senha = senhaHash;

            await _usuarioRepositorio.AtualizarAsync(usuario);
        }


        public async Task RemoverAsync(int usuarioId)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
            if (usuario == null) throw new Exception("Usuário não encontrado.");

            usuario.Deletar();
            await _usuarioRepositorio.AtualizarAsync(usuario);
        }

        public async Task RestaurarAsync(int usuarioId)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
            if (usuario == null || usuario.Ativo)
                throw new Exception("Usuário não encontrado ou já ativo.");

            usuario.Restaurar();
            await _usuarioRepositorio.AtualizarAsync(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> ListarTodosAsync()
        {
            var usuarios = await _usuarioRepositorio.ListarTodosAsync();
            return usuarios.Select(u => new UsuarioDTO
            {
                UsuarioID = u.UsuarioID,
                Nome = u.Nome,
                Email = u.Email,
                ImagemPerfilUrl = u.ImagemPerfilUrl,
                IdiomaSelecionadoCodigo = u.IdiomaSelecionadoCodigo,
                TipoUsuario = u.TipoUsuario,
                XP = u.XP,
                Senha = null
            });
        }


        public async Task<string> LogarAsync(string email, string senha)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmailAsync(email);
            ValidarUsuarioAtivo(usuario);

            var resultado = _passwordHasher.VerifyHashedPassword(email, usuario.Senha, senha);
            if (resultado == PasswordVerificationResult.Failed)
                throw new Exception("Credenciais inválidas.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfiguracoes.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(_jwtConfiguracoes.ExpiracaoEmHoras),
                Issuer = _jwtConfiguracoes.Emissor,
                Audience = _jwtConfiguracoes.Publico,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UsuarioDTO?> ValidateUser(string email, string senha)
        {
            // 1. Buscar o usuário pelo username no banco (repositório)
            var user = await _usuarioRepositorio.ObterPorEmailAsync(email);
            if (user == null || !user.Ativo)
                return null;

            // 2. Verificar a senha (comparar hash)
            var resultado = _passwordHasher.VerifyHashedPassword(email, user.Senha, senha);
            if (resultado == PasswordVerificationResult.Failed)
                return null;

            // 3. Mapear para DTO e retornar
            return new UsuarioDTO
            {
                UsuarioID = user.UsuarioID,
                Nome = user.Nome,
                Email = user.Email,
                TipoUsuario = user.TipoUsuario
            };
        }

        public async Task<UsuarioDTO> ObterPorIdAsync(int usuarioId)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
            if (usuario == null || !usuario.Ativo) return null;

            return new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                ImagemPerfilUrl = usuario.ImagemPerfilUrl,
                IdiomaSelecionadoCodigo = usuario.IdiomaSelecionadoCodigo,
                TipoUsuario = usuario.TipoUsuario,
                Senha = null
            };
        }

        public async Task<UsuarioDTO> ObterPorEmailAsync(string email)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmailAsync(email);
            if (usuario == null || !usuario.Ativo) return null;

            return new UsuarioDTO
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                ImagemPerfilUrl = usuario.ImagemPerfilUrl,
                IdiomaSelecionadoCodigo = usuario.IdiomaSelecionadoCodigo,
                TipoUsuario = usuario.TipoUsuario,
                Senha = null
            };
        }



        public async Task AtualizarImagemPerfilAsync(int usuarioId, string imagemPerfilUrl)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            usuario.ImagemPerfilUrl = imagemPerfilUrl;
            await _usuarioRepositorio.AtualizarAsync(usuario);
        }


        private void ValidarUsuarioAtivo(Usuario? usuario)
        {
            if (usuario == null || !usuario.Ativo)
                throw new Exception("Usuário não encontrado ou inativo.");
        }

        public async Task<bool> AtualizarXPAsync(int usuarioId, int incrementoXP)
        {
            var usuario = await _usuarioRepositorio.ObterPorIdAsync(usuarioId);
            if (usuario == null || !usuario.Ativo)
                return false;

            usuario.XP += incrementoXP;

            await _usuarioRepositorio.AtualizarAsync(usuario);
            return true;
        }

        public async Task<Dictionary<string, int>> ObterXpPorIdiomaAsync(int usuarioId)
        {
            return await _usuarioRepositorio.ObterXPPorIdiomaAsync(usuarioId);
        }

    }
}
