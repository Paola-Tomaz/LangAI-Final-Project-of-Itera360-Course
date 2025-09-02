namespace LangAI.Aplicacao.Configuracoes
{
    public class JwtConfiguracoes
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Emissor { get; set; } = string.Empty;
        public string Publico { get; set; } = string.Empty;
        public int ExpiracaoEmHoras { get; set; }
    }
}
