using NetCoreAuthSeed.Dominio;
using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;

namespace NetCoreAuthSeed.Repositorio
{
    public class RepUsuario : IRepUsuario
    {

        private readonly AppDbContext _context;

        public RepUsuario(AppDbContext context)
        {
            _context = context;
        }

        public int InserirDTO(Usuario novoUsuario)
        {
            _context.Add(novoUsuario);
            _context.SaveChanges();
            return novoUsuario.Id;
        }

        public  Usuario RecuperarPorUserName(string username) 
        {
            return  _context.Set<Usuario>().FirstOrDefault(u => u.Username == username);
        }
    }
}