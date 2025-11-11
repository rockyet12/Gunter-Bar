using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

// Implementación del servicio de bebidas
public class DrinkService : IDrinkService
{
    private readonly IDrinkRepository _drinkRepository;

    public DrinkService(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<ApiResponse<IEnumerable<DrinkDto>>> GetAllAsync()
    {
        try
        {
            var drinks = await _drinkRepository.GetAllAsync();

            var drinkDtos = drinks.Select(d => new DrinkDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Stock = d.Stock,
                Ingredients = d.Ingredients.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            });

            return ApiResponse<IEnumerable<DrinkDto>>.Succeed(drinkDtos, "Bebidas obtenidas exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<DrinkDto>>.Fail($"Error al obtener las bebidas: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<DrinkDto>>> GetByTypeAsync(DrinkType type)
    {
        try
        {
            var drinks = await _drinkRepository.GetByTypeAsync(type);

            var drinkDtos = drinks.Select(d => new DrinkDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Stock = d.Stock,
                Ingredients = d.Ingredients.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            });

            return ApiResponse<IEnumerable<DrinkDto>>.Succeed(drinkDtos, "Bebidas filtradas obtenidas exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<DrinkDto>>.Fail($"Error al obtener las bebidas por tipo: {ex.Message}");
        }
    }

    public async Task<ApiResponse<DrinkDto>> GetByIdAsync(int id)
    {
        try
        {
            var drink = await _drinkRepository.GetByIdAsync(id);

            if (drink == null)
            {
                return ApiResponse<DrinkDto>.Fail("Bebida no encontrada");
            }

            var drinkDto = new DrinkDto
            {
                Id = drink.Id,
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                Stock = drink.Stock,
                Ingredients = drink.Ingredients.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            return ApiResponse<DrinkDto>.Succeed(drinkDto, "Bebida obtenida exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<DrinkDto>.Fail($"Error al obtener la bebida: {ex.Message}");
        }
    }

    public async Task<ApiResponse<DrinkDto>> CreateAsync(CreateDrinkDto createDrinkDto)
    {
        try
        {
            var drink = new Drink(createDrinkDto.Name, createDrinkDto.Price, createDrinkDto.Stock, createDrinkDto.Type, createDrinkDto.Description);

            var createdDrink = await _drinkRepository.CreateAsync(drink);

            var drinkDto = new DrinkDto
            {
                Id = createdDrink.Id,
                Name = createdDrink.Name,
                Description = createdDrink.Description,
                Price = createdDrink.Price,
                Stock = createdDrink.Stock,
                Ingredients = createdDrink.Ingredients.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            return ApiResponse<DrinkDto>.Succeed(drinkDto, "Bebida creada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<DrinkDto>.Fail($"Error al crear la bebida: {ex.Message}");
        }
    }

    public async Task<ApiResponse<DrinkDto>> UpdateAsync(int id, UpdateDrinkDto updateDrinkDto)
    {
        try
        {
            var drink = await _drinkRepository.GetByIdAsync(id);

            if (drink == null)
            {
                return ApiResponse<DrinkDto>.Fail("Bebida no encontrada");
            }

            drink.Name = updateDrinkDto.Name;
            drink.Description = updateDrinkDto.Description;
            drink.Price = updateDrinkDto.Price;
            drink.Stock = updateDrinkDto.Stock;

            var updatedDrink = await _drinkRepository.UpdateAsync(drink);

            var drinkDto = new DrinkDto
            {
                Id = updatedDrink.Id,
                Name = updatedDrink.Name,
                Description = updatedDrink.Description,
                Price = updatedDrink.Price,
                Stock = updatedDrink.Stock,
                Ingredients = updatedDrink.Ingredients.Select(i => new DrinkIngredientDto 
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList()
            };

            return ApiResponse<DrinkDto>.Succeed(drinkDto, "Bebida actualizada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<DrinkDto>.Fail($"Error al actualizar la bebida: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var drink = await _drinkRepository.GetByIdAsync(id);

            if (drink == null)
            {
                return ApiResponse<bool>.Fail("Bebida no encontrada");
            }

            await _drinkRepository.DeleteAsync(id);
            return ApiResponse<bool>.Succeed(true, "Bebida eliminada exitosamente");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al eliminar la bebida: {ex.Message}");
        }
    }
}
