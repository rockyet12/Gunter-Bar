using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

// Implementación del servicio de órdenes
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IDrinkRepository _drinkRepository;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IDrinkRepository drinkRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _drinkRepository = drinkRepository;
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(int userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        var orderDtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            UserId = o.UserId,
            UserFullName = o.User?.FullName ?? "",
            Total = o.Total,
            Status = o.Status.ToString(),
            CreatedAt = o.CreatedAt,
            Notes = o.Notes,
            Items = o.Items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.DrinkName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        });

        return new ApiResponse<IEnumerable<OrderDto>>
        {
            Success = true,
            Message = "Órdenes obtenidas exitosamente",
            Data = orderDtos
        };
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        var orderDtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            UserId = o.UserId,
            UserFullName = o.User.FullName,
            Total = o.Total,
            Status = o.Status.ToString(),
            CreatedAt = o.CreatedAt,
            Notes = o.Notes,
            Items = o.Items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.DrinkName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        });

        return new ApiResponse<IEnumerable<OrderDto>>
        {
            Success = true,
            Message = "Todas las órdenes obtenidas exitosamente",
            Data = orderDtos
        };
    }

    public async Task<ApiResponse<OrderDto>> GetOrderByIdAsync(int orderId, int userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Orden no encontrada",
                Errors = { "Order not found" }
            };
        }

        // Verificar que el usuario pueda acceder a esta orden
        if (order.UserId != userId)
        {
            return new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "No tienes acceso a esta orden",
                Errors = { "Access denied" }
            };
        }

        var orderDto = new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserFullName = order.User.FullName,
            Total = order.Total,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            Notes = order.Notes,
            Items = order.Items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.DrinkName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        };

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Orden obtenida exitosamente",
            Data = orderDto
        };
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderFromCartAsync(int userId, CreateOrderDto createOrderDto)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null || !cart.Items.Any())
        {
            return new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Carrito vacío o no encontrado",
                Errors = { "Cart is empty or not found" }
            };
        }

        // Verificar stock disponible
        foreach (var cartItem in cart.Items)
        {
            var drink = await _drinkRepository.GetByIdAsync(cartItem.DrinkId);
            if (drink == null || !drink.IsAvailable || drink.Stock < cartItem.Quantity)
            {
                return new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = $"Stock insuficiente para {cartItem.Drink.Name}",
                    Errors = { "Insufficient stock" }
                };
            }
        }

        // Crear la orden
        var order = new Order
        {
            UserId = userId,
            Total = cart.Total,
            Status = OrderStatus.Pendiente,
            Notes = createOrderDto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        // Crear los items de la orden
        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem
            {
                DrinkId = cartItem.DrinkId,
                DrinkName = cartItem.Drink.Name,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice
            };
            order.Items.Add(orderItem);

            // Actualizar stock
            await _drinkRepository.UpdateStockAsync(cartItem.DrinkId, cartItem.Drink.Stock - cartItem.Quantity);
        }

        var createdOrder = await _orderRepository.CreateAsync(order);

        // Limpiar el carrito
        await _cartRepository.ClearCartAsync(cart.Id);

        var orderDto = new OrderDto
        {
            Id = createdOrder.Id,
            UserId = createdOrder.UserId,
            Total = createdOrder.Total,
            Status = createdOrder.Status.ToString(),
            CreatedAt = createdOrder.CreatedAt,
            Notes = createdOrder.Notes,
            Items = createdOrder.Items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.DrinkName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        };

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Orden creada exitosamente",
            Data = orderDto
        };
    }

    public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateStatusDto)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            return new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Orden no encontrada",
                Errors = { "Order not found" }
            };
        }

        order.Status = (OrderStatus)updateStatusDto.Status;
        var updatedOrder = await _orderRepository.UpdateAsync(order);

        var orderDto = new OrderDto
        {
            Id = updatedOrder.Id,
            UserId = updatedOrder.UserId,
            UserFullName = updatedOrder.User.FullName,
            Total = updatedOrder.Total,
            Status = updatedOrder.Status.ToString(),
            CreatedAt = updatedOrder.CreatedAt,
            Notes = updatedOrder.Notes,
            Items = updatedOrder.Items.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                DrinkId = oi.DrinkId,
                DrinkName = oi.DrinkName,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Subtotal = oi.Subtotal
            }).ToList()
        };

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Estado de orden actualizado exitosamente",
            Data = orderDto
        };
    }
}
