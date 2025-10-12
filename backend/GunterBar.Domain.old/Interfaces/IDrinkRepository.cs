using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de bebidas
public interface IDrinkRepository
{
    Task<Drink?> GetByIdAsync(Guid id);
    Task<IEnumerable<Drink>> GetAllAsync();
    Task<IEnumerable<Drink>> GetByTypeAsync(DrinkType type);
    Task<IEnumerable<Drink>> GetAvailableAsync();
    Task<Drink> CreateAsync(Drink drink);
    Task<Drink> UpdateAsync(Drink drink);
    Task DeleteAsync(Guid id);
    Task<bool> UpdateStockAsync(Guid drinkId, int newStock);
}
