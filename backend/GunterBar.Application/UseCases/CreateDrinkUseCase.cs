using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.UseCases;

public class CreateDrinkUseCase
{
    private readonly IDrinkRepository _drinkRepository;

    public CreateDrinkUseCase(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task<DrinkDto> ExecuteAsync(DrinkDto drinkDto)
    {
        var drink = new Drink
        {
            Name = drinkDto.Name,
            Description = drinkDto.Description,
            Price = drinkDto.Price,
            Type = Enum.Parse<DrinkType>(drinkDto.Type),
            ImageUrl = drinkDto.ImageUrl,
            IsAvailable = drinkDto.IsAvailable,
            Stock = drinkDto.Stock
        };

        await _drinkRepository.AddAsync(drink);
        return drinkDto;
    }
}
