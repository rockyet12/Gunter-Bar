using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Cart;

public record AddToCartRequest(int UserId, AddToCartDto Item);

public class AddToCartUseCase : UseCase<AddToCartRequest, CartDto>
{
    private readonly ICartService _cartService;

    public AddToCartUseCase(ICartService cartService)
    {
        _cartService = cartService;
    }

    protected override async Task<ApiResponse<CartDto>> ExecuteAsync(AddToCartRequest request)
    {
        if (request.UserId <= 0)
        {
            return ApiResponse<CartDto>.Fail("ID de usuario inválido");
        }

        if (request.Item == null)
        {
            return ApiResponse<CartDto>.Fail("Item no proporcionado");
        }

        if (request.Item.Quantity <= 0)
        {
            return ApiResponse<CartDto>.Fail("La cantidad debe ser mayor a 0");
        }

        if (request.Item.DrinkId <= 0)
        {
            return ApiResponse<CartDto>.Fail("ID de bebida inválido");
        }

        try
        {
            return await _cartService.AddToCartAsync(request.UserId, request.Item);
        }
        catch (Exception ex)
        {
            return ApiResponse<CartDto>.Fail($"Error al agregar al carrito: {ex.Message}");
        }
    }
}
