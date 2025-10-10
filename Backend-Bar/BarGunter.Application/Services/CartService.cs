using BarGunter.Application.Interfaces.IRepositories;
using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;

namespace BarGunter.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        return await _cartRepository.GetAllAsync();
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _cartRepository.GetByIdAsync(id);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        return await _cartRepository.CreateAsync(cart);
    }

    public async Task<Cart?> UpdateAsync(int id, Cart cart)
    {
        return await _cartRepository.UpdateAsync(id, cart);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _cartRepository.DeleteAsync(id);
    }
}

