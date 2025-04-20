using Microsoft.EntityFrameworkCore;
using BarGunter.Domain.Entities;

namespace BarGunter.Infrastructure.Persistences;

public class BarGunterDbContext : DbContext
{
    public BarGunterDbContext(DbContextOptions<BarGunterDbContext> options)
        : base(options)
    {
    }

    // Define tus DbSet para las entidades
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());

        // Configuración adicional de las entidades (si es necesario)
    }
}


//dotnet ef migrations  add firstmigration --context BarGunterDbContext --project .\BarGunter.Infrastructure\ --startup-project .\BarGunter.API\ --output-dir Persistences/Migrations
//dotnet ef database update --context BarGunterDbContext --project .\BarGunter.Infrastructure\ --startup-project .\BarGunter.API\