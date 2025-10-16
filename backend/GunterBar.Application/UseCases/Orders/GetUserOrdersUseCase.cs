using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Orders;

public record GetUserOrdersRequest(int UserId);

public class GetUserOrdersUseCase : UseCase<GetUserOrdersRequest, IEnumerable<OrderDto>>
{
    private readonly IOrderService _orderService;

    public GetUserOrdersUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    protected override async Task<ApiResponse<IEnumerable<OrderDto>>> ExecuteAsync(GetUserOrdersRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                return ApiResponse<IEnumerable<OrderDto>>.Fail("ID de usuario inválido");
            }

            return await _orderService.GetUserOrdersAsync(request.UserId);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<OrderDto>>.Fail($"Error al obtener las órdenes del usuario: {ex.Message}");
        }
    }
}
