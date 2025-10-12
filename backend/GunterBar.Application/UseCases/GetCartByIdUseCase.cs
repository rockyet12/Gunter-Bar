using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetCartByIdUseCase
{
    private readonly ICartService _cartService;

    public GetCartByIdUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<CartDto> ExecuteAsync(Guid id)
    {
        return await _cartService.GetCartByIdAsync(id);
    }
}
