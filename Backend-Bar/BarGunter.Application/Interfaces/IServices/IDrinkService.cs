using BarGunter.Domain.Entities;

namespace BarGunter.Application.Interfaces.IServices;
    public interface IDrinkService
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<Drink> CreateAsync(Drink drink);
        Task<Drink?> UpdateAsync(int id, Drink drink);
        Task<bool> DeleteAsync(int id);
    }

