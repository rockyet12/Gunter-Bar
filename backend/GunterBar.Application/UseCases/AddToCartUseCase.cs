using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class AddToCartUseCase
{
    private readonly ICartService _cartService;

    public AddToCartUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<CartDto> ExecuteAsync(Guid cartId, CartItemDto item)
    {
        // Añadir el item al carrito
        await _cartService.AddItemToCartAsync(cartId, item);
        
        // Devolver el carrito actualizado
        return await _cartService.GetCartByIdAsync(cartId);
    }
}
