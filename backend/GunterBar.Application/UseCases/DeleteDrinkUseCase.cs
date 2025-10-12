using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class DeleteDrinkUseCase
{
    private readonly IDrinkRepository _drinkRepository;

    public DeleteDrinkUseCase(IDrinkRepository drinkRepository)
    {
        _drinkRepository = drinkRepository;
    }

    public async Task ExecuteAsync(Guid id)
    {
        await _drinkRepository.DeleteAsync(id);
    }
}
