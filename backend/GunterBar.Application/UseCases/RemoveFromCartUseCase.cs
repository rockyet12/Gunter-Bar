using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class RemoveFromCartUseCase
{
    private readonly ICartService _cartService;

    public RemoveFromCartUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<CartDto> ExecuteAsync(Guid cartId, Guid drinkId)
    {
        // Eliminar el item del carrito
        await _cartService.RemoveItemFromCartAsync(cartId, drinkId);
        
        // Devolver el carrito actualizado
        return await _cartService.GetCartByIdAsync(cartId);
    }
}
