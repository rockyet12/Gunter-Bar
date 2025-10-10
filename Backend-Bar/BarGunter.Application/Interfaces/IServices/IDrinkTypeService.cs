using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IServices;
    public interface IDrinkTypeService
    {
        Task<IEnumerable<DrinkType>> GetAllAsync();
        Task<DrinkType?> GetByIdAsync(int id);
        Task<DrinkType> CreateAsync(DrinkType drinkType);
        Task<DrinkType?> UpdateAsync(int id, DrinkType drinkType);
        Task<bool> DeleteAsync(int id);
    }

