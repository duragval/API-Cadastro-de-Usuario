using CadastroUsuarioWebApi.Models;

namespace CadastroUsuarioWebApi.Repositories
{
    public interface ICadastroRepository
    {
        IEnumerable<Cadastro> GetAll();
        Cadastro GetCadastro(int id);
        Cadastro Create (Cadastro cadastro);
        Cadastro Update (Cadastro cadastro);
        Cadastro Delete(int id);
        Task<Cadastro> GetByEmailAsync(string email);
    }
}
