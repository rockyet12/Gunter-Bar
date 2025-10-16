using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

public interface IDrinkIngredientRepository
{
    Task<DrinkIngredient?> GetByIdAsync(int id);
    Task<IEnumerable<DrinkIngredient>> GetByDrinkIdAsync(int drinkId);
    Task<DrinkIngredient> CreateAsync(DrinkIngredient ingredient);
    Task<DrinkIngredient> UpdateAsync(DrinkIngredient ingredient);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
