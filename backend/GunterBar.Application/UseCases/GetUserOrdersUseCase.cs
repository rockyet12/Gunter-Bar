using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;

namespace GunterBar.Application.UseCases;

public class GetUserOrdersUseCase
{
    private readonly IOrderService _orderService;

    public GetUserOrdersUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IEnumerable<OrderDto>> ExecuteAsync(Guid userId)
    {
        return await _orderService.GetUserOrdersAsync(userId);
    }
}
