namespace GunterBar.Application.DTOs;

public class DrinkIngredientDto
{
    public Guid DrinkId { get; set; }
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; }
}
