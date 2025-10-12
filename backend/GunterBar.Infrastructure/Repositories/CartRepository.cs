using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GunterBar.Infrastructure.Repositories;

// Implementación del repositorio de carrito
public class CartRepository : ICartRepository
{
    private readonly GunterBarDbContext _context;

    public CartRepository(GunterBarDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Drink)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        cart.UpdatedAt = DateTime.UtcNow;
        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int drinkId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.DrinkId == drinkId);
    }

    public async Task<CartItem> AddItemAsync(CartItem cartItem)
    {
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }

    public async Task<CartItem> UpdateItemAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }

    public async Task DeleteItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int cartId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();
        
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }
}
