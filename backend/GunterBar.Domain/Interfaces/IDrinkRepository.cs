using GunterBar.Domain.Entities;
using GunterBar.Domain;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de bebidas
public interface IDrinkRepository
{
    Task<Drink> GetByIdAsync(int id);
    Task<IEnumerable<Drink>> GetAllAsync();
    Task<IEnumerable<Drink>> GetByTypeAsync(DrinkType type);
    Task<IEnumerable<Drink>> GetAvailableAsync();
    Task<Drink> CreateAsync(Drink drink);
    Task<Drink> UpdateAsync(Drink drink);
    Task DeleteAsync(int id);
    Task<bool> UpdateStockAsync(int drinkId, int newStock);
}
