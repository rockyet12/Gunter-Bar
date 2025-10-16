using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;

namespace GunterBar.Application.UseCases.Orders;

public record GetOrderByIdRequest(int OrderId, int UserId);

public class GetOrderByIdUseCase : UseCase<GetOrderByIdRequest, OrderDto>
{
    private readonly IOrderService _orderService;

    public GetOrderByIdUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    protected override async Task<ApiResponse<OrderDto>> ExecuteAsync(GetOrderByIdRequest request)
    {
        try
        {
            if (request.OrderId <= 0)
            {
                return ApiResponse<OrderDto>.Fail("El ID de la orden no es válido");
            }

            if (request.UserId <= 0)
            {
                return ApiResponse<OrderDto>.Fail("El ID del usuario no es válido");
            }

            return await _orderService.GetOrderByIdAsync(request.OrderId, request.UserId);
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderDto>.Fail($"Error al obtener la orden: {ex.Message}");
        }
    }
}
