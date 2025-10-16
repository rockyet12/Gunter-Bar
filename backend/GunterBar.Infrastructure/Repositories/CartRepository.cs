using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;
using GunterBar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GunterBar.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly GunterBarDbContext _context;

    public CartRepository(GunterBarDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("El ID del usuario debe ser mayor que 0", nameof(userId));

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado");

        return await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Drink)
                    .ThenInclude(d => d.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        if (cart == null)
            throw new ArgumentNullException(nameof(cart));

        // Verificar si el usuario ya tiene un carrito
        var existingCart = await GetByUserIdAsync(cart.UserId);
        if (existingCart != null)
            throw new InvalidOperationException($"El usuario {cart.UserId} ya tiene un carrito activo");

        // Verificar que el usuario existe
        var userExists = await _context.Users.AnyAsync(u => u.Id == cart.UserId);
        if (!userExists)
            throw new KeyNotFoundException($"Usuario con ID {cart.UserId} no encontrado");

        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        if (cart == null)
            throw new ArgumentNullException(nameof(cart));

        var existingCart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cart.Id);

        if (existingCart == null)
            throw new KeyNotFoundException($"Carrito con ID {cart.Id} no encontrado");

        // Actualizar propiedades del carrito
        _context.Entry(existingCart).CurrentValues.SetValues(cart);

        // Actualizar items
        if (cart.Items != null)
        {
            // Eliminar items que ya no existen
            foreach (var existingItem in existingCart.Items.ToList())
            {
                if (!cart.Items.Any(i => i.Id == existingItem.Id))
                {
                    existingCart.Items.Remove(existingItem);
                }
            }

            // Actualizar o agregar nuevos items
            foreach (var itemDetail in cart.Items)
            {
                var existingItem = existingCart.Items
                    .FirstOrDefault(i => i.Id == itemDetail.Id);

                if (existingItem == null)
                {
                    // Verificar que la bebida existe y tiene stock suficiente
                    var drink = await _context.Drinks.FindAsync(itemDetail.DrinkId);
                    if (drink == null)
                        throw new KeyNotFoundException($"Bebida con ID {itemDetail.DrinkId} no encontrada");

                    if (drink.Stock < itemDetail.Quantity)
                        throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");

                    existingCart.Items.Add(itemDetail);
                }
                else
                {
                    // Verificar stock si la cantidad aumentó
                    if (itemDetail.Quantity > existingItem.Quantity)
                    {
                        var drink = await _context.Drinks.FindAsync(itemDetail.DrinkId);
                        if (drink != null && drink.Stock < (itemDetail.Quantity - existingItem.Quantity))
                            throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");
                    }
                    _context.Entry(existingItem).CurrentValues.SetValues(itemDetail);
                }
            }
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existingCart;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int drinkId)
    {
        return await _context.CartItems
            .Include(ci => ci.Drink)
            .AsNoTracking()
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.DrinkId == drinkId);
    }

    public async Task<CartItem> AddItemAsync(CartItem cartItem)
    {
        if (cartItem == null)
            throw new ArgumentNullException(nameof(cartItem));

        // Verificar que el carrito existe
        var cart = await _context.Carts.FindAsync(cartItem.CartId);
        if (cart == null)
            throw new KeyNotFoundException($"Carrito con ID {cartItem.CartId} no encontrado");

        // Verificar que la bebida existe y tiene stock suficiente
        var drink = await _context.Drinks.FindAsync(cartItem.DrinkId);
        if (drink == null)
            throw new KeyNotFoundException($"Bebida con ID {cartItem.DrinkId} no encontrada");

        if (drink.Stock < cartItem.Quantity)
            throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");

        // Verificar si ya existe el item en el carrito
        var existingItem = await GetCartItemAsync(cartItem.CartId, cartItem.DrinkId);
        if (existingItem != null)
        {
            existingItem.Quantity += cartItem.Quantity;
            await _context.SaveChangesAsync();
            return existingItem;
        }

        // Si no existe, agregar nuevo item
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }

    public async Task<CartItem> UpdateItemAsync(CartItem cartItem)
    {
        if (cartItem == null)
            throw new ArgumentNullException(nameof(cartItem));

        var existingItem = await _context.CartItems
            .Include(ci => ci.Drink)
            .FirstOrDefaultAsync(ci => ci.Id == cartItem.Id);

        if (existingItem == null)
            throw new KeyNotFoundException($"Item de carrito con ID {cartItem.Id} no encontrado");

        // Verificar stock si la cantidad aumentó
        if (cartItem.Quantity > existingItem.Quantity)
        {
            var drink = await _context.Drinks.FindAsync(existingItem.DrinkId);
            if (drink != null && drink.Stock < (cartItem.Quantity - existingItem.Quantity))
                throw new InvalidOperationException($"Stock insuficiente para la bebida {drink.Name}");
        }

        _context.Entry(existingItem).CurrentValues.SetValues(cartItem);
        await _context.SaveChangesAsync();
        return existingItem;
    }

    public async Task DeleteItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem == null)
            throw new KeyNotFoundException($"Item de carrito con ID {cartItemId} no encontrado");

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task ClearCartAsync(int cartId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
            throw new KeyNotFoundException($"Carrito con ID {cartId} no encontrado");

        cart.Items.Clear();
        await _context.SaveChangesAsync();
    }
}
