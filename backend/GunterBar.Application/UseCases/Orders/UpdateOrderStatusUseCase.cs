using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Application.UseCases.Common;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.UseCases.Orders;

public record UpdateOrderStatusRequest(int OrderId, OrderStatus NewStatus);

public class UpdateOrderStatusUseCase : UseCase<UpdateOrderStatusRequest, OrderDto>
{
    private readonly IOrderService _orderService;

    public UpdateOrderStatusUseCase(IOrderService orderService)
    {
        _orderService = orderService;
    }

    protected override async Task<ApiResponse<OrderDto>> ExecuteAsync(UpdateOrderStatusRequest request)
    {
        try
        {
            if (request.OrderId <= 0)
            {
                return ApiResponse<OrderDto>.Fail("El ID de la orden no es válido");
            }

            var updateStatusDto = new UpdateOrderStatusDto
            {
                OrderId = request.OrderId,
                NewStatus = request.NewStatus
            };

            return await _orderService.UpdateOrderStatusAsync(request.OrderId, updateStatusDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderDto>.Fail($"Error al actualizar el estado de la orden: {ex.Message}");
        }
    }
}
