using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases;

public class CreateOrderFromCartUseCase
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public CreateOrderFromCartUseCase(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    public async Task<OrderDto> ExecuteAsync(Guid cartId)
    {
        var cart = await _cartService.GetCartByIdAsync(cartId);
        
        var orderDto = new OrderDto
        {
            UserId = cart.UserId,
            Status = OrderStatus.Created.ToString(),
            CreatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(i => new OrderItemDto
            {
                DrinkId = i.DrinkId,
                Quantity = i.Quantity
            }).ToList()
        };

        var createdOrder = await _orderService.CreateOrderAsync(orderDto);
        
        // Limpiar el carrito después de crear la orden
        await _cartService.ClearCartAsync(cartId);

        return createdOrder;
    }
}
