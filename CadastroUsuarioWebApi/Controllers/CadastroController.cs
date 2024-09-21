using AutoMapper;
using CadastroUsuarioWebApi.Context;
using CadastroUsuarioWebApi.DTOs;
using CadastroUsuarioWebApi.Models;
using CadastroUsuarioWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuarioWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CadastroController> _logger;
        private readonly IMapper _mapper;

        public CadastroController(IUnitOfWork uof, ILogger<CadastroController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<CadastroDTO>> Get()
        {
            _logger.LogInformation($"GET request recebido em {DateTime.Now}");
            var cadastros = _uof.CadastroRepository.GetAll();

            _logger.LogInformation($"Total de cadastros retornados: {cadastros.Count()}");
            var cadastrosDto = _mapper.Map<IEnumerable<CadastroDTO>>(cadastros);
            return Ok(cadastrosDto);
        }

        [Authorize]
        [HttpGet("{id:int:min(1)}", Name = "ObterCadastro")]
        public async Task<ActionResult<CadastroDTO>> Get(int id)
        {
            _logger.LogInformation($"GET request de id = {id} recebido em {DateTime.Now}");
            var cadastro = _uof.CadastroRepository.GetCadastro(id);

            if (cadastro == null)
            {
                _logger.LogWarning($"cadastro com id = {id} não encontrado : {DateTime.Now}");
                return NotFound($"cadastro com id = {id} não encontrado");
            }
            var cadastroDto = _mapper.Map<CadastroDTO>(cadastro);
            return Ok(cadastroDto);

        }

        
        [HttpPost]
        public ActionResult<CadastroDTO> Post(CadastroDTO cadastroDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"Dados invalidos para POST em {DateTime.Now}");
                return BadRequest(ModelState);
            }
            try
            {
                _logger.LogInformation($"POST request recebido em {DateTime.Now}");

                var cadastro = _mapper.Map<Cadastro>(cadastroDto);
                if (cadastro.Senha != null)
                {
                    cadastro.SetSenhaHash();
                }

                var cadastroCriado = _uof.CadastroRepository.Create(cadastro);
                _uof.Commit();

                var cadastroCriadoDto = _mapper.Map<CadastroDTO>(cadastroCriado);

                _logger.LogInformation($"Cadastro criado com sucesso. ID = {cadastroCriadoDto.Id} ," +
                    $"{DateTime.Now}");

                return new CreatedAtRouteResult("ObterCadastro",
                    new { id = cadastroCriadoDto.Id }, cadastroCriadoDto);
            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao criar cadastro em {DateTime.Now}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar seu cadastro");
            }

        }

        [Authorize]
        [HttpPut("{id:int:min(1)}")]
        public ActionResult<CadastroDTO> Put(int id, CadastroDTO cadastroDto)
        {
            try
            {
                _logger.LogInformation($"PUT request para ID = {id} recebido em {DateTime.Now}");
                if (id != cadastroDto.Id)
                {
                    _logger.LogWarning($"ID do cadastro ({id}) não corresponde ao ID na url");
                    return BadRequest("O ID do cadastro não corresponde ao ID fornecido");
                }

                var atualizaCad = _uof.CadastroRepository.GetCadastro(id);
                if (atualizaCad == null)
                {
                    _logger.LogWarning($"Cadastro com id = {id} não encontrado");
                    return NotFound($"Cadastro com id = {id} não encontrado");
                }

                var cadastro = _mapper.Map<Cadastro>(cadastroDto);

                atualizaCad.Nome = cadastro.Nome;
                atualizaCad.Email = cadastro.Email;

                var cadAtualizado = _uof.CadastroRepository.Update(atualizaCad);
                _uof.Commit();

                _logger.LogInformation($"Cadastro do ID{id} atualizado com sucesso em {DateTime.Now}");

                var cadAtualizadoDto = _mapper.Map<CadastroDTO>(cadAtualizado);
                return Ok(cadAtualizadoDto);
            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao atualizar Cadastro {DateTime.Now}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Ocorreu um problema ao tratar sua atualização");

            }

        }

        [Authorize]
        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<CadastroDTO> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"DELETE request recebido em {DateTime.Now}");
                //var cadastro = _context.Cadastros.FirstOrDefault(x => x.Id == id);
                var cadastro = _uof.CadastroRepository.GetCadastro(id);

                if (cadastro == null)
                {
                    _logger.LogWarning($"Id {id} não encontrado");
                    return NotFound($"Id {id} não encontrado");
                }

                var cadastroDeletado = _uof.CadastroRepository.Delete(id);
                _uof.Commit();

                _logger.LogInformation($"Cadastro com id = {id} deletado em {DateTime.Now}");

                var cadastroDeletadoDto = _mapper.Map<CadastroDTO>(cadastro);
                return Ok(cadastroDeletadoDto);
            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao deletar cadastro de id {id} em {DateTime.Now}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Ocorreu um problema ao deletar o usuário");
            }

        }
    }
}
