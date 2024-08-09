using CadastroUsuarioWebApi.Helper;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CadastroUsuarioWebApi.Models;

public class Cadastro : IValidatableObject
{
    public Cadastro()
    {
        Cadastros = new Collection<Cadastro>();
    }

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do usuário")]
    [StringLength(100)]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Informe seu email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Senha { get; set; }

    [JsonIgnore]
    public ICollection<Cadastro> Cadastros { get; set; }

    public void SetSenhaHash()
    {
        Senha = Senha.GerarHash();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Nome))
        {
            var primeiraLetra = this.Nome[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                yield return new
                    ValidationResult("A primeira letra do nome deve ser maiúscula",
                    new[] { nameof(this.Nome) });
            }
        }
    }
}