using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GunterBar.Domain.Entities;

public class DrinkIngredient
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey("Drink")]
    public int DrinkId { get; set; }

    public virtual Drink Drink { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    // Constructor
    public DrinkIngredient(int drinkId, string name, int quantity)
    {
        DrinkId = drinkId;
        Name = name;
        Quantity = quantity;
    }
}