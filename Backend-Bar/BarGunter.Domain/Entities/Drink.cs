namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class Drink
{
    [Key]
    public int DrinkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TypeId { get; set; }

    // Navigation property
    public DrinkType? DrinkType { get; set; }

    public Drink() { }

    public Drink(int drinkId, string name, string description, decimal price, int typeId)
    {
        DrinkId = drinkId;
        Name = name;
        Description = description;
        Price = price;
        TypeId = typeId;
    }
}
