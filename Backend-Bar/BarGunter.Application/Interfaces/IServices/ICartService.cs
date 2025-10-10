using BarGunter.Domain.Entities;

namespace BarGunter.Application.Contracts.IServices;
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(int id);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart?> UpdateAsync(int id, Cart cart);
        Task<bool> DeleteAsync(int id);
    }

