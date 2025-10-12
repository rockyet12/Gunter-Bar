using GunterBar.Domain.Entities;

namespace GunterBar.Domain.ValueObjects;

public class DrinkIngredientDetails
{
    public string Name { get; }
    public decimal Quantity { get; }
    public string Unit { get; }

    public DrinkIngredientDetails(string name, decimal quantity, string unit)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (string.IsNullOrWhiteSpace(unit))
            throw new ArgumentException("Unit cannot be empty", nameof(unit));

        Name = name;
        Quantity = quantity;
        Unit = unit;
    }
}
