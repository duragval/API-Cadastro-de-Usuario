using CadastroUsuarioWebApi.Context;
using CadastroUsuarioWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuarioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cadastro>>> Get()
        {
            try
            {
                var cadastros = await _context.Cadastros.AsNoTracking().ToListAsync();
                if (cadastros is null)
                {
                    return NotFound("Cadastros não encontrados");
                }
                return cadastros;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCadastro")]
        public async Task<ActionResult<Cadastro>> Get(int id)
        {
            try
            {
                var cadastro = await _context.Cadastros.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (cadastro is null)
                {
                    return NotFound("Cadastro não encontrado");
                }
                return cadastro;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação");
            }

        }

        [HttpPost]

        public async Task<ActionResult> Post(Cadastro cadastro)
        {
            try
            {
                if (cadastro.Senha != null)
                {
                    cadastro.SetSenhaHash();
                }

                await _context.Cadastros.AddAsync(cadastro);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCadastro",
                    new { id = cadastro.Id }, cadastro);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar seu cadastro");
            }

        }

        [HttpPut("{id:int:min(1)}")]

        public async Task<ActionResult> Put(int id, Cadastro cadastro)
        {
            try
            {
                if (id != cadastro.Id)
                {
                    return BadRequest();
                }

                var atualizaCad = await _context.Cadastros.FindAsync(id);
                if (atualizaCad == null)
                {
                    return NotFound();
                }

                atualizaCad.Nome = cadastro.Nome;
                atualizaCad.Email = cadastro.Email;

                _context.Entry(atualizaCad).Property(x => x.Nome).IsModified = true;
                _context.Entry(atualizaCad).Property(x => x.Email).IsModified = true;


                await _context.SaveChangesAsync();

                return Ok(atualizaCad);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Ocorreu um problema ao tratar sua atualização");

            }

        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //var cadastro = _context.Cadastros.FirstOrDefault(x => x.Id == id);
                var cadastro = await _context.Cadastros.FindAsync(id);

                if (cadastro is null)
                {
                    return NotFound($"Id {id} não encontrado");
                }

                _context.Cadastros.Remove(cadastro);
                await _context.SaveChangesAsync();

                return Ok(cadastro);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Ocorreu um problema ao deletar o usuário");
            }

        }
    }
}
