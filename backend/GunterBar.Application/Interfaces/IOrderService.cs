using GunterBar.Application.DTOs;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.Interfaces;

// Servicio de órdenes
public interface IOrderService
{
    Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(int userId);
    Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync(); // Solo admin
    Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, int userId);
    Task<ApiResponse<OrderDto>> CreateOrderFromCartAsync(int userId, CreateOrderDto createOrderDto);
    Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateStatusDto); // Solo admin
}
