using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;

namespace NetCoreAuthSeed.Dominio
{
    public class UsuarioDTO
    {
        public  string Username { get; set; }
        public  string Email { get; set; }
        public  string Password { get; set; }
        public void ToClass(Usuario usuario)
        {
            usuario.Username = Username;
            usuario.Email = Email;
            usuario.Password = Password;
        }
    }
}
