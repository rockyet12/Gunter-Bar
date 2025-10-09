using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IRepositories;
    public interface IDrinkRepository
    {
        Task<IEnumerable<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(int id);
        Task<Drink> CreateAsync(Drink drink);
        Task<Drink?> UpdateAsync(int id, Drink drink);
        Task<bool> DeleteAsync(int id);
    }

