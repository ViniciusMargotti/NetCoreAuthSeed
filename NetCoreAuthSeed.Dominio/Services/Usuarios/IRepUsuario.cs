using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;

namespace NetCoreAuthSeed.Dominio
{
    public interface IRepUsuario
    {
        public int InserirDTO(Usuario novoUsuario);
        public Usuario RecuperarPorUserName(string username);
    }
}
