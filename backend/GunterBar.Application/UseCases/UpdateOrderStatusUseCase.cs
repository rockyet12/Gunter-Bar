using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases;

public class UpdateOrderStatusUseCase
{
    private readonly IOrderService _orderService;

    public UpdateOrderStatusUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId, OrderStatus newStatus)
    {
        return await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
    }
}
