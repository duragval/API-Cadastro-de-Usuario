using CadastroUsuarioWebApi.Context;

namespace CadastroUsuarioWebApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public ICadastroRepository? _cadastroRepo;
    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICadastroRepository CadastroRepository
    {
        get
        {
            return _cadastroRepo = _cadastroRepo ?? new CadastroRepository(_context);
        }
    }

    public void Commit()
    {
       _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
