using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetOrderByIdUseCase
{
    private readonly IOrderService _orderService;

    public GetOrderByIdUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<OrderDto> ExecuteAsync(Guid orderId)
    {
        return await _orderService.GetOrderByIdAsync(orderId);
    }
}
