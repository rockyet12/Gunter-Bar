using GunterBar.Domain.Entities;

namespace GunterBar.Domain.Interfaces;

// Contrato para repositorio de carrito
public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);
    Task<Cart> CreateAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
    Task<CartItem?> GetCartItemAsync(int cartId, int drinkId);
    Task<CartItem> AddItemAsync(CartItem cartItem);
    Task<CartItem> UpdateItemAsync(CartItem cartItem);
    Task DeleteItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
}
