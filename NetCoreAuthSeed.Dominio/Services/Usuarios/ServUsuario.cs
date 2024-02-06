using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;
using NetCoreAuthSeed.NetCoreAuthSeed.Repositories.Repositorios.Usuarios;

namespace NetCoreAuthSeed.Dominio
{
    public class ServUsuario : IServUsuario
    {
        private readonly IRepUsuario _repUsuario;

        public ServUsuario(IRepUsuario repUsuario)
        {
            _repUsuario = repUsuario;
        }

        public int InserirDTO(UsuarioDTO dto)
        {
            var novoUsuario = new Usuario();
            dto.ToClass(novoUsuario);
            return _repUsuario.InserirDTO(novoUsuario);
        }
    }
}
