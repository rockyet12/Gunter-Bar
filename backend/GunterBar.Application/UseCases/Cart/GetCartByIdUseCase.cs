using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Cart;

public record GetCartByIdRequest(int UserId);

public class GetCartByIdUseCase : UseCase<GetCartByIdRequest, CartDto>
{
    private readonly ICartService _cartService;

    public GetCartByIdUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    protected override async Task<ApiResponse<CartDto>> ExecuteAsync(GetCartByIdRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<CartDto>.Fail("ID de usuario inválido");
            }

            var cart = await _cartService.GetCartAsync(request.UserId);
            if (!cart.Success)
            {
                return ApiResponse<CartDto>.Fail(cart.Message);
            }

            return cart;
        }
        catch (Exception ex)
        {
            return ApiResponse<CartDto>.Fail($"Error al obtener el carrito: {ex.Message}");
        }
    }
}
