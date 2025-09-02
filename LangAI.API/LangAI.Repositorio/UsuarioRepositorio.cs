using LangAI.Dominio.Entidades;
using LangAI.Repositorio.Interfaces;
using LangAI.Repositorio.Contextos;
using Microsoft.EntityFrameworkCore;

namespace LangAI.Repositorio.Contextos;

public class UsuarioRepositorio : BaseRepositorio, IUsuarioRepositorio
{
    public UsuarioRepositorio(LangAIContexto contexto) : base(contexto)
    {
    }

    public async Task<int> SalvarAsync(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();
        return usuario.UsuarioID;
    }

    public async Task AtualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto
            .SaveChangesAsync();
    }

    public async Task RestaurarAsync(Usuario usuario)
    {
        usuario.Restaurar();
        _contexto.Usuarios.Update(usuario);
        _contexto.SaveChangesAsync();
    }


    public async Task<Usuario> ObterPorIdAsync(int usuarioId)
    {
        return await _contexto.Usuarios
        .FirstOrDefaultAsync(u => u.UsuarioID == usuarioId && u.Ativo);
    }

    public async Task<Usuario> ObterDeletadoAsync(int usuarioId)
    {
        return await _contexto.Usuarios
            .Where(u => u.UsuarioID == usuarioId && u.Ativo == false)
            .FirstOrDefaultAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _contexto.Usuarios
            .Where(u => u.Email == email && u.Ativo)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Usuario>> ListarTodosAsync()
    {
        return await _contexto.Usuarios
            .Where(u => u.Ativo)
            .ToListAsync();
    }

    public async Task<bool> AtualizarXPAsync(int usuarioId, int incrementoXP)
    {
        var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.UsuarioID == usuarioId && u.Ativo);
        if (usuario == null)
            return false;

        usuario.XP += incrementoXP;

        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<Dictionary<string, int>> ObterXPPorIdiomaAsync(int usuarioId)
    {
        var usuario = await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UsuarioID == usuarioId && u.Ativo);

        if (usuario == null)
            return new Dictionary<string, int>();

        return new Dictionary<string, int>
    {
        { "ingles", usuario.XPIngles },
        { "japones", usuario.XPJapones },
        { "frances", usuario.XPFrances },
        { "alemao", usuario.XPAlemao }
    };
    }

}