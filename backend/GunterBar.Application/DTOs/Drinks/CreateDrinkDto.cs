using System.ComponentModel.DataAnnotations;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Drinks;

public class CreateDrinkDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es requerido")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "El tipo es requerido")]
    public DrinkType Type { get; set; }

    [StringLength(2000, ErrorMessage = "La URL de la imagen no puede tener más de 2000 caracteres")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "El stock es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [StringLength(50, ErrorMessage = "La categoría no puede tener más de 50 caracteres")]
    public string Category { get; set; } = "Sin categoría";

    public List<DrinkIngredientDto> Ingredients { get; set; } = new();
}
