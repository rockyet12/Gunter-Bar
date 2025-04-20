using BarGunter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd(); // Generar el Id automáticamente
            
        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(70);
        // Configura otras propiedades según sea necesario
    }
}