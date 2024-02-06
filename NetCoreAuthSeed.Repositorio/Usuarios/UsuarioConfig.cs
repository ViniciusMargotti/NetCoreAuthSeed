using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetCoreAuthSeed.NetCoreAuthSeed.Domain.Models.Usuarios;

namespace NetCoreAuthSeed.Repositorio
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");

            builder.HasKey(b => b.Id)
                   .HasName("idusuario");

            builder.Property(e => e.Id)
                   .HasColumnName("idusuario")
                   .IsRequired();

            builder.Property(e => e.Username)
                   .HasColumnName("username")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(e => e.Email)
                   .HasColumnName("email")
                   .HasMaxLength(300)
                   .IsRequired();

            builder.Property(e => e.Password)
                   .HasColumnName("password")
                   .HasMaxLength(8)
                   .IsRequired();
        }
    }
}
