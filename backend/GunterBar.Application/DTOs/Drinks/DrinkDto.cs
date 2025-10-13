using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Drinks;

public class DrinkDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DrinkType Type { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int Stock { get; set; }
    public List<DrinkIngredientDto> Ingredients { get; set; } = new();
}
