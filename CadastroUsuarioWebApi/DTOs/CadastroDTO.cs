using System.ComponentModel.DataAnnotations;

namespace CadastroUsuarioWebApi.DTOs;

public class CadastroDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do usuário")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe seu email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Senha { get; set; }
}
