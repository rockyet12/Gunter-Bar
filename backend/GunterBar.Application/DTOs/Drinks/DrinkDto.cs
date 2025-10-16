using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Drinks;

public class DrinkDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DrinkType Type { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public string Category { get; set; } = "Sin categoría";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<DrinkIngredientDto> Ingredients { get; set; } = new();
}
