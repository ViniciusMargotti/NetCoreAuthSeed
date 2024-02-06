using NetCoreAuthSeed.Dominio;

namespace NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios
{
    public class Usuario : Identificador
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
