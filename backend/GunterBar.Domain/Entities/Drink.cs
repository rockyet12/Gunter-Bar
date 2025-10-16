using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Common;

namespace GunterBar.Domain.Entities;

public class Drink : EntityBase
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public bool IsAvailable { get; set; }

    [MaxLength(2000)]
    public string? ImageUrl { get; set; }

    public string Category { get; set; } = "Sin categoría";

    // Navigation properties
    public virtual ICollection<DrinkIngredient> Ingredients { get; set; } = new List<DrinkIngredient>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // Constructor
    protected Drink() { }

    public Drink(string name, decimal price, int stock, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es requerido", nameof(name));
        
        if (price <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0", nameof(price));
        
        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo", nameof(stock));

        Name = name.Trim();
        Price = price;
        Stock = stock;
        Description = description?.Trim() ?? string.Empty;
        IsAvailable = stock > 0;
    }

    // Methods
    public void UpdateStock(int quantity)
    {
        if (Stock + quantity < 0)
            throw new InvalidOperationException("Stock insuficiente");

        Stock += quantity;
        IsAvailable = Stock > 0;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("El precio debe ser mayor a 0", nameof(newPrice));

        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }
}