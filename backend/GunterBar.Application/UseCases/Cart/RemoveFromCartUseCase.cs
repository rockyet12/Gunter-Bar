using GunterBar.Application.Common.Models;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Cart;

public record RemoveFromCartRequest(int UserId, int CartItemId);

public class RemoveFromCartUseCase : UseCase<RemoveFromCartRequest, bool>
{
    private readonly ICartService _cartService;

    public RemoveFromCartUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    protected override async Task<ApiResponse<bool>> ExecuteAsync(RemoveFromCartRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<bool>.Fail("ID de usuario inválido");
            }

            if (request.CartItemId <= 0)
            {
                return ApiResponse<bool>.Fail("ID de item inválido");
            }

            return await _cartService.RemoveFromCartAsync(request.UserId, request.CartItemId);
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.Fail($"Error al eliminar el item del carrito: {ex.Message}");
        }
    }
}
