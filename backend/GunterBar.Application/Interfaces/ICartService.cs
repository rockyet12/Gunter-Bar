using GunterBar.Application.DTOs;

namespace GunterBar.Application.Interfaces;

// Servicio de carrito
public interface ICartService
{
    Task<ApiResponse<CartDto>> GetCartAsync(int userId);
    Task<ApiResponse<CartDto>> AddToCartAsync(int userId, AddToCartDto addToCartDto);
    Task<ApiResponse<CartDto>> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto updateDto);
    Task<ApiResponse<bool>> RemoveFromCartAsync(int userId, int cartItemId);
    Task<ApiResponse<bool>> ClearCartAsync(int userId);
}
