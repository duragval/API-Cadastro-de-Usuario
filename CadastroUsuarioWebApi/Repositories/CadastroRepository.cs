using CadastroUsuarioWebApi.Context;
using CadastroUsuarioWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuarioWebApi.Repositories
{
    public class CadastroRepository : ICadastroRepository
    {
        private readonly AppDbContext _context;

        public CadastroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cadastro> GetByEmailAsync(string email)
        {
            return await _context.Cadastros.SingleOrDefaultAsync(c => c.Email == email);
        }

        public IEnumerable<Cadastro> GetAll()
        {
            return _context.Cadastros.AsNoTracking().ToList();
        }
        public Cadastro GetCadastro(int id)
        {
            return _context.Cadastros.AsNoTracking().FirstOrDefault(c => c.Id == id);

        }
        public Cadastro Create(Cadastro cadastro)
        {
            if (cadastro == null)
            {
                throw new ArgumentNullException(nameof(cadastro));
            }

            _context.Cadastros.Add(cadastro);
            //_context.SaveChanges();

            return cadastro;

        }
        public Cadastro Update(Cadastro cadastro)
        {
            if (cadastro is null)
            {
                throw new ArgumentNullException(nameof(cadastro));
            }

            _context.Entry(cadastro).State = EntityState.Modified;
            //_context.SaveChanges();

            return cadastro;
        }
        public Cadastro Delete(int id)
        {
            var cadastro = _context.Cadastros.Find(id);
            if (cadastro is null)
            {
                throw new ArgumentException(nameof(cadastro));
            }

            _context.Cadastros.Remove(cadastro);
            //_context.SaveChanges();

            return cadastro;
        }
    }
}
