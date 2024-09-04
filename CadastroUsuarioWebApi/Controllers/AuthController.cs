using CadastroUsuarioWebApi.DTOs;
using CadastroUsuarioWebApi.Helper;
using CadastroUsuarioWebApi.Repositories;
using CadastroUsuarioWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CadastroUsuarioWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _uof;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, IUnitOfWork uof, IConfiguration config, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _uof = uof;
        _config = config;
        _logger = logger;
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        _logger.LogInformation($"Tentativa de login do user {loginDto.Email} em {DateTime.Now}");
        var usuario = await _uof.CadastroRepository.GetByEmailAsync(loginDto.Email);

        if (usuario == null || !Criptografia.VerificarSenha(loginDto.Senha, usuario.Senha))
        {
            _logger.LogWarning($"Login do user {loginDto.Email} falhou em {DateTime.Now}" +
                $"(Email ou senha incorretos)");
            return Unauthorized(new { message = "Email ou senha incorretos" });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        var accessToken = _tokenService.GenerateAcessToken(claims, _config);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _logger.LogInformation($"Login feito com sucesso! Usuario: {loginDto.Email} em {DateTime.Now}");

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            Expiration = accessToken.ValidTo.ToLocalTime()
        });
    }
}