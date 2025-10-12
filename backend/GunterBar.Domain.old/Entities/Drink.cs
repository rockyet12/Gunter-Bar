using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunterBar.Domain.Entities;

public class Drink
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    // Navigation properties
    public virtual ICollection<DrinkIngredient> Ingredients { get; set; } = new List<DrinkIngredient>();

    // Constructor
    public Drink(string name, decimal price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
    }
}