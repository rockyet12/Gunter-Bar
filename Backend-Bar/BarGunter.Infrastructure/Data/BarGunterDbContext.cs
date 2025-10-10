using Microsoft.EntityFrameworkCore;
using BarGunter.Domain.Entities;

namespace BarGunter.Infrastructure.Persistences;

public class BarGunterDbContext : DbContext
{
    public BarGunterDbContext(DbContextOptions<BarGunterDbContext> options) : base(options)
    {
    }

    // English entity DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<DrinkType> DrinkTypes { get; set; }
    public DbSet<Drink> Drinks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Remove Usuario configuration since it's deleted
        // Configure entity relationships if needed
        
        // Product - Category relationship
        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        // Drink - DrinkType relationship  
        modelBuilder.Entity<Drink>()
            .HasOne<DrinkType>()
            .WithMany()
            .HasForeignKey(d => d.TypeId);

        // Order - Ticket relationship
        modelBuilder.Entity<Order>()
            .HasOne<Ticket>()
            .WithMany()
            .HasForeignKey(o => o.TicketId);

        // Cart - Product relationship
        modelBuilder.Entity<Cart>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(c => c.ProductId);
    }
}