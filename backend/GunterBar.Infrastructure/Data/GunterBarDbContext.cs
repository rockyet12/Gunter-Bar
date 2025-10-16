using Microsoft.EntityFrameworkCore;
using GunterBar.Domain.Entities;

namespace GunterBar.Infrastructure.Data;

public class GunterBarDbContext : DbContext
{
    public GunterBarDbContext(DbContextOptions<GunterBarDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Drink> Drinks { get; set; } = null!;
    public DbSet<DrinkIngredient> DrinkIngredients { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GunterBarDbContext).Assembly);
    }
}
