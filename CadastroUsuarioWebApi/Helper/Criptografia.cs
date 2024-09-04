using System.Security.Cryptography;
using System.Text;

namespace CadastroUsuarioWebApi.Helper;

public static class Criptografia
{
    public static string GerarHash(this string valor)
    {
        return BCrypt.Net.BCrypt.HashPassword(valor);
    }

    public static bool VerificarSenha(string senha, string hashArmazenado)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hashArmazenado);
    }
}
