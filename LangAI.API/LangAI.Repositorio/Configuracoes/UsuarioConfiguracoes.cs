using LangAI.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LangAI.Repositorio.Configuracoes;

public class UsuarioConfiguracoes : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.UsuarioID);

        builder.Property(u => u.UsuarioID).HasColumnName("UsuarioID");
        builder.Property(u => u.Nome).HasColumnName("Nome").IsRequired();
        builder.Property(u => u.Email).HasColumnName("Email").IsRequired();
        builder.Property(u => u.Senha).HasColumnName("Senha").IsRequired();
        builder.Property(u => u.TipoUsuario).IsRequired();
        builder.Property(u => u.Ativo).IsRequired();
        builder.Property(u => u.ImagemPerfilUrl);
        builder.Property(u => u.IdiomaSelecionadoCodigo);

        // Relacionamento com Idioma
        builder.HasOne(u => u.IdiomaSelecionado)
               .WithMany()
               .HasForeignKey(u => u.IdiomaSelecionadoCodigo);
    }
}
