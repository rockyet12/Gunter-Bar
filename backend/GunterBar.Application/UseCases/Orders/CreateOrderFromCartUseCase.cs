using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Orders;

public record CreateOrderFromCartRequest(int UserId, CreateOrderDto OrderData);

public class CreateOrderFromCartUseCase : UseCase<CreateOrderFromCartRequest, OrderDto>
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public CreateOrderFromCartUseCase(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    protected override async Task<ApiResponse<OrderDto>> ExecuteAsync(CreateOrderFromCartRequest request)
    {
        if (request.UserId <= 0)
        {
            return ApiResponse<OrderDto>.Fail("ID de usuario inválido");
        }

        // Obtener el carrito actual del usuario
        var cartResponse = await _cartService.GetCartAsync(request.UserId);
        if (!cartResponse.Success)
        {
            return ApiResponse<OrderDto>.Fail(cartResponse.Message);
        }

        if (cartResponse.Data?.Items == null || !cartResponse.Data.Items.Any())
        {
            return ApiResponse<OrderDto>.Fail("El carrito está vacío");
        }

        // Crear la orden
        var orderResponse = await _orderService.CreateOrderFromCartAsync(request.UserId, request.OrderData);
        if (!orderResponse.Success)
        {
            return orderResponse;
        }

        // Limpiar el carrito
        await _cartService.ClearCartAsync(request.UserId);

        return orderResponse;
    }
}
