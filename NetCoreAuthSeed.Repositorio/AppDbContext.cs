using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;

namespace NetCoreAuthSeed.Repositorio;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UsuarioConfig());
        base.OnModelCreating(builder);
    }

    public DbSet<Usuario> Usuario { get; set; }

}
