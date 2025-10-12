using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetDrinkByIdUseCase
{
    private readonly IDrinkService _drinkService;

    public GetDrinkByIdUseCase(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    public async Task<DrinkDto> ExecuteAsync(Guid drinkId)
    {
        return await _drinkService.GetDrinkByIdAsync(drinkId);
    }
}
