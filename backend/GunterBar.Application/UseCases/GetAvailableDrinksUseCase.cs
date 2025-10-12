using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.UseCases;

/// <summary>
/// Caso de uso para obtener las bebidas disponibles.
/// </summary>
public class GetAvailableDrinksUseCase
{
    private readonly IDrinkRepository _drinkRepository;

    public GetAvailableDrinksUseCase(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    /// <summary>
    /// Ejecuta el caso de uso para obtener las bebidas disponibles.
    /// </summary>
    /// <returns>Una lista de bebidas disponibles.</returns>
    public async Task<IEnumerable<DrinkDto>> ExecuteAsync()
    {
        var drinks = await _drinkRepository.GetAvailableAsync();

        return drinks.Select(d => new DrinkDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description,
            Price = d.Price,
            Type = d.Type.ToString(),
            ImageUrl = d.ImageUrl,
            IsAvailable = d.IsAvailable,
            Stock = d.Stock
        });
    }
}
