using LangAI.Dominio.Entidades;
using LangAI.Dominio.Enumeradores;

public class Usuario
{
    public int UsuarioID { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public string ImagemPerfilUrl { get; set; } = string.Empty;
    public string IdiomaSelecionadoCodigo { get; set; } = string.Empty;
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public string Descricao { get; set; }

    public int XP { get; set; } = 0;
    public int XPIngles { get; set; }
    public int XPJapones { get; set; }
    public int XPFrances { get; set; }
    public int XPAlemao { get; set; }

    public bool EhAdmin { get; set; } = false;

    public Idioma IdiomaSelecionado { get; set; }

    public TipoUsuarioEnum TipoUsuario { get; set; }

    public void Deletar() => Ativo = false;
    public void Restaurar() => Ativo = true;
}
