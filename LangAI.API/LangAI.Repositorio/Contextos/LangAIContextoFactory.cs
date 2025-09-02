using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace LangAI.Repositorio.Contextos
{
    public class LangAIContextoFactory : IDesignTimeDbContextFactory<LangAIContexto>
    {
        public LangAIContexto CreateDbContext(string[] args)
        {
            // Caminho para o appsettings.json do projeto de startup (API)
            var caminhoBase = Directory.GetCurrentDirectory();
            var caminhoApi = Path.Combine(caminhoBase, "../LangAI.API");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(caminhoApi)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LangAIContexto>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new LangAIContexto(optionsBuilder.Options);
        }
    }
}
