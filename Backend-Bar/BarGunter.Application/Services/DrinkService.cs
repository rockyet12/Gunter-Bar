using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;

public class DrinkService : IDrinkService
{
    private readonly IDrinkRepository _drinkRepository;

        public DrinkService(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

    public async Task<IEnumerable<Drink>> GetAllAsync()
    {
        return await _drinkRepository.GetAllAsync();
    }

    public async Task<Drink?> GetByIdAsync(int id)
    {
        return await _drinkRepository.GetByIdAsync(id);
    }

    public async Task<Drink> CreateAsync(Drink drink)
    {
        return await _drinkRepository.CreateAsync(drink);
    }

    public async Task<Drink?> UpdateAsync(int id, Drink drink)
    {
        return await _drinkRepository.UpdateAsync(id, drink);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _drinkRepository.DeleteAsync(id);
    }
}

