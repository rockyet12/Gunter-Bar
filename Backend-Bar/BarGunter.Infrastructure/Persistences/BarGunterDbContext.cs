using Microsoft.EntityFrameworkCore;
using BarGunter.Domain.Entities;

namespace BarGunter.Infrastructure.Persistences;

public class BarGunterDbContext : DbContext
{
    public BarGunterDbContext(DbContextOptions<BarGunterDbContext> options) : base(options)
    {
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Tipo> Tipos { get; set; }
    public DbSet<Tragos> Tragos { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<Login> Logins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
    }
}