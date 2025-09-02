using LangAI.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class IdiomaConfiguracoes : IEntityTypeConfiguration<Idioma>
{
    public void Configure(EntityTypeBuilder<Idioma> builder)
    {
        builder.ToTable("Idiomas");

        builder.HasKey(i => i.Codigo);

        builder.HasData(
            new Idioma { Codigo = "pt-BR", Nome = "Português" },
            new Idioma { Codigo = "en-US", Nome = "Inglês" },
            new Idioma { Codigo = "fr-CA", Nome = "Francês" },
            new Idioma { Codigo = "de-DE", Nome = "Alemão" },
            new Idioma { Codigo = "ja-JP", Nome = "Japonês" }
        );
    }
}