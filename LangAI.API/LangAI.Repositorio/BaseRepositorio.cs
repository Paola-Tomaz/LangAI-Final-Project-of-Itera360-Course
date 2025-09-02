using LangAI.Repositorio.Contextos;

public abstract class BaseRepositorio
{
    protected readonly LangAIContexto _contexto;

    protected BaseRepositorio(LangAIContexto contexto)
    {
        _contexto = contexto;
    }
}
