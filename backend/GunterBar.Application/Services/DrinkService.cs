using GunterBar.Application.DTOs;
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
        var drinks = await _drinkRepository.GetAllAsync();

        var drinkDtos = drinks.Select(d => new DrinkDto
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

        return new ApiResponse<IEnumerable<DrinkDto>>
        {
            Success = true,
            Message = "Bebidas obtenidas exitosamente",
            Data = drinkDtos
        };
    }

    public async Task<ApiResponse<IEnumerable<DrinkDto>>> GetByTypeAsync(DrinkType type)
    {
        var drinks = await _drinkRepository.GetByTypeAsync(type);

        var drinkDtos = drinks.Select(d => new DrinkDto
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

        return new ApiResponse<IEnumerable<DrinkDto>>
        {
            Success = true,
            Message = "Bebidas filtradas obtenidas exitosamente",
            Data = drinkDtos
        };
    }

    public async Task<ApiResponse<DrinkDto>> GetByIdAsync(int id)
    {
        var drink = await _drinkRepository.GetByIdAsync(id);

        if (drink == null)
        {
            return new ApiResponse<DrinkDto>
            {
                Success = false,
                Message = "Bebida no encontrada",
                Errors = { "Drink not found" }
            };
        }

        return new ApiResponse<DrinkDto>
        {
            Success = true,
            Message = "Bebida obtenida exitosamente",
            Data = new DrinkDto
            {
                Id = drink.Id,
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                Type = drink.Type.ToString(),
                ImageUrl = drink.ImageUrl,
                IsAvailable = drink.IsAvailable,
                Stock = drink.Stock
            }
        };
    }

    public async Task<ApiResponse<DrinkDto>> CreateAsync(CreateDrinkDto createDrinkDto)
    {
        var drink = new Drink
        {
            Name = createDrinkDto.Name,
            Description = createDrinkDto.Description,
            Price = createDrinkDto.Price,
            Type = (DrinkType)createDrinkDto.Type,
            ImageUrl = createDrinkDto.ImageUrl,
            Stock = createDrinkDto.Stock,
            IsAvailable = true
        };

        var createdDrink = await _drinkRepository.CreateAsync(drink);

        return new ApiResponse<DrinkDto>
        {
            Success = true,
            Message = "Bebida creada exitosamente",
            Data = new DrinkDto
            {
                Id = createdDrink.Id,
                Name = createdDrink.Name,
                Description = createdDrink.Description,
                Price = createdDrink.Price,
                Type = createdDrink.Type.ToString(),
                ImageUrl = createdDrink.ImageUrl,
                IsAvailable = createdDrink.IsAvailable,
                Stock = createdDrink.Stock
            }
        };
    }

    public async Task<ApiResponse<DrinkDto>> UpdateAsync(int id, CreateDrinkDto updateDrinkDto)
    {
        var drink = await _drinkRepository.GetByIdAsync(id);

        if (drink == null)
        {
            return new ApiResponse<DrinkDto>
            {
                Success = false,
                Message = "Bebida no encontrada",
                Errors = { "Drink not found" }
            };
        }

        drink.Name = updateDrinkDto.Name;
        drink.Description = updateDrinkDto.Description;
        drink.Price = updateDrinkDto.Price;
        drink.Type = (DrinkType)updateDrinkDto.Type;
        drink.ImageUrl = updateDrinkDto.ImageUrl;
        drink.Stock = updateDrinkDto.Stock;
        drink.UpdatedAt = DateTime.UtcNow;

        var updatedDrink = await _drinkRepository.UpdateAsync(drink);

        return new ApiResponse<DrinkDto>
        {
            Success = true,
            Message = "Bebida actualizada exitosamente",
            Data = new DrinkDto
            {
                Id = updatedDrink.Id,
                Name = updatedDrink.Name,
                Description = updatedDrink.Description,
                Price = updatedDrink.Price,
                Type = updatedDrink.Type.ToString(),
                ImageUrl = updatedDrink.ImageUrl,
                IsAvailable = updatedDrink.IsAvailable,
                Stock = updatedDrink.Stock
            }
        };
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        var drink = await _drinkRepository.GetByIdAsync(id);

        if (drink == null)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Bebida no encontrada",
                Errors = { "Drink not found" }
            };
        }

        await _drinkRepository.DeleteAsync(id);

        return new ApiResponse<bool>
        {
            Success = true,
            Message = "Bebida eliminada exitosamente",
            Data = true
        };
    }
}
