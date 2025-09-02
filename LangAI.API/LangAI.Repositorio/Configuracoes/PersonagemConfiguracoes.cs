using LangAI.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PersonagemConfiguracoes : IEntityTypeConfiguration<Personagem>
{
    public void Configure(EntityTypeBuilder<Personagem> builder)
    {
        builder.ToTable("Personagens");
        builder.HasKey(p => p.PersonagemID);

        builder.HasData(
            new Personagem { PersonagemID = 1, Nome = "Lola", ImagemUrl = "", FraseApresentacao = "Olá, sou a Lola e vou te ensinar Português!", IdiomaCodigo = "pt-BR" },
            new Personagem { PersonagemID = 2, Nome = "Tyler", ImagemUrl = "", FraseApresentacao = "Hi, I'm Tyler and i'll teach you English!", IdiomaCodigo = "en-US" },
            new Personagem { PersonagemID = 3, Nome = "Sebastian", ImagemUrl = "", FraseApresentacao = "Bonjour, je suis Sebastian et je vais vous apprendre le français!", IdiomaCodigo = "fr-CA" },
            new Personagem { PersonagemID = 4, Nome = "Klaus", ImagemUrl = "", FraseApresentacao = "Hallo, ich bin Klaus und ich bringe dir Französisch bei!", IdiomaCodigo = "de-DE" },
            new Personagem { PersonagemID = 5, Nome = "Miyuki", ImagemUrl = "", FraseApresentacao = "こんにちは、I’m Miyuki と I’l が日本語をお教えします！", IdiomaCodigo = "ja-JP" }
        );

        builder.HasOne(p => p.Idioma)
       .WithMany(i => i.Personagens)
       .HasForeignKey(p => p.IdiomaCodigo);

    }
}