using System.ComponentModel.DataAnnotations;

namespace CadastroUsuarioWebApi.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Email é obrigatório")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Senha é obrigatória")]
    public string? Senha { get; set; }
}
