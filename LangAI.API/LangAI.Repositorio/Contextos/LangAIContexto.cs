using LangAI.Dominio.Entidades;
using LangAI.Repositorio.Configuracoes;
using Microsoft.EntityFrameworkCore;
using LangAI.Repositorio.Contextos;

namespace LangAI.Repositorio.Contextos
{
    public class LangAIContexto : DbContext
    {
        public LangAIContexto(DbContextOptions<LangAIContexto> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<Personagem> Personagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
            modelBuilder.ApplyConfiguration(new IdiomaConfiguracoes());
            modelBuilder.ApplyConfiguration(new PersonagemConfiguracoes());
        }
    }
}