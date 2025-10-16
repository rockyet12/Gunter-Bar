using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.UseCases.Common;
using GunterBar.Domain.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases.Drinks;

public record GetAvailableDrinksRequest;

/// <summary>
/// Caso de uso para obtener las bebidas disponibles.
/// </summary>
public class GetAvailableDrinksUseCase : UseCase<GetAvailableDrinksRequest, IEnumerable<DrinkDto>>
{
    private readonly IDrinkRepository _drinkRepository;

    public GetAvailableDrinksUseCase(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    /// <summary>
    /// Ejecuta el caso de uso para obtener las bebidas disponibles.
    /// </summary>
    /// <returns>Una respuesta con la lista de bebidas disponibles.</returns>
    protected override async Task<ApiResponse<IEnumerable<DrinkDto>>> ExecuteAsync(GetAvailableDrinksRequest request)
    {
        try
        {
            var drinks = await _drinkRepository.GetAvailableAsync();
            
            if (drinks == null || !drinks.Any())
            {
                return ApiResponse<IEnumerable<DrinkDto>>.Succeed(Enumerable.Empty<DrinkDto>(), "No hay bebidas disponibles");
            }
            
            var drinksDto = drinks.Select(d => new DrinkDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Stock = d.Stock,
                Ingredients = d.Ingredients?.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList() ?? new List<DrinkIngredientDto>()
            }).ToList();

            return ApiResponse<IEnumerable<DrinkDto>>.Succeed(drinksDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<DrinkDto>>.Fail($"Error al obtener las bebidas disponibles: {ex.Message}");
        }
    }
}
