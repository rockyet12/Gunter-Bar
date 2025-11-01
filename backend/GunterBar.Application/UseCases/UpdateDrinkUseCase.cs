using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases;

public class UpdateDrinkUseCase
{
    private readonly IDrinkRepository _drinkRepository;

    public UpdateDrinkUseCase(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<DrinkDto> ExecuteAsync(int id, DrinkDto drinkDto)
    {
        var drink = await _drinkRepository.GetByIdAsync(id);
        if (drink == null)
        {
            throw new KeyNotFoundException($"Bebida con ID {id} no encontrada.");
        }

        drink.Name = drinkDto.Name;
        drink.Description = drinkDto.Description;
        drink.Price = drinkDto.Price;
        drink.Type = drinkDto.Type;
        drink.ImageUrl = drinkDto.ImageUrl;
        drink.IsAvailable = drinkDto.IsAvailable;
        drink.Stock = drinkDto.Stock;

        await _drinkRepository.UpdateAsync(drink);
        return drinkDto;
    }
}
