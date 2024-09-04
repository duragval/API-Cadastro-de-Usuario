namespace CadastroUsuarioWebApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICadastroRepository CadastroRepository { get; }

    void Commit();
}
