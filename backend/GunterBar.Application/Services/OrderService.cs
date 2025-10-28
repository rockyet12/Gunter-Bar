using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Enums;
using GunterBar.Domain.Interfaces;
using GunterBar.Domain.Common;

namespace GunterBar.Application.Services;

// Implementación del servicio de órdenes
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IDrinkRepository _drinkRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    
    private const int MAX_ITEMS_PER_ORDER = 20;
    private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedStatusTransitions = new()
    {
        { OrderStatus.Pending, new[] { OrderStatus.InProgress, OrderStatus.Cancelled } },
        { OrderStatus.InProgress, new[] { OrderStatus.Completed, OrderStatus.Cancelled } },
        { OrderStatus.Completed, Array.Empty<OrderStatus>() },
        { OrderStatus.Cancelled, Array.Empty<OrderStatus>() }
    };

    public OrderService(
        IOrderRepository orderRepository, 
        ICartRepository cartRepository, 
        IDrinkRepository drinkRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _drinkRepository = drinkRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    private static OrderDto MapToOrderDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            UserName = order.User?.Name ?? "",
            Total = order.Total.Amount,
            Status = order.Status,
            Notes = order.Notes,
            OrderDate = order.OrderDate,
            CompletedDate = order.CompletedDate,
            CancelledDate = order.CancelledDate,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
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
    }

    public async Task<ApiResponse<IEnumerable<OrderDto>>> GetUserOrdersAsync(int userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        var orderDtos = orders.Select(MapToOrderDto);

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

        var orderDtos = orders.Select(MapToOrderDto);

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

        var orderDto = MapToOrderDto(order);

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Orden obtenida exitosamente",
            Data = orderDto
        };
    }

    public async Task<ApiResponse<OrderDto>> CreateOrderFromCartAsync(int userId, CreateOrderDto createOrderDto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                return ApiResponse<OrderDto>.Fail("Carrito vacío o no encontrado");
            }

            if (cart.Items.Count > MAX_ITEMS_PER_ORDER)
            {
                return ApiResponse<OrderDto>.Fail($"El carrito excede el límite de {MAX_ITEMS_PER_ORDER} items");
            }

            // Verificar stock disponible y precios actualizados
            foreach (var cartItem in cart.Items)
            {
                if (cartItem.Quantity <= 0)
                {
                    return ApiResponse<OrderDto>.Fail($"La cantidad debe ser mayor a 0 para {cartItem.Drink.Name}");
                }

                var drink = await _drinkRepository.GetByIdAsync(cartItem.DrinkId);
                if (drink == null || !drink.IsAvailable)
                {
                    return ApiResponse<OrderDto>.Fail($"La bebida {cartItem.Drink.Name} no está disponible");
                }

                if (drink.Stock < cartItem.Quantity)
                {
                    return ApiResponse<OrderDto>.Fail($"Stock insuficiente para {cartItem.Drink.Name}. Disponible: {drink.Stock}");
                }

                if (drink.Price != cartItem.UnitPrice)
                {
                    return ApiResponse<OrderDto>.Fail($"El precio de {cartItem.Drink.Name} ha cambiado. Por favor, actualice el carrito");
                }
            }

        // Crear la orden con datos extendidos
        var order = new Order(
            userId,
            createOrderDto.Notes,
            createOrderDto.Direccion,
            createOrderDto.MetodoPago,
            createOrderDto.Tarjeta,
            createOrderDto.CodigoVerif
        );

        // Crear los items de la orden
        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem(
                order.Id,
                cartItem.DrinkId, 
                cartItem.Drink.Name,
                cartItem.Quantity,
                cartItem.UnitPrice
            );
            order.AddItem(orderItem);

            // Actualizar stock
            await _drinkRepository.UpdateStockAsync(cartItem.DrinkId, cartItem.Drink.Stock - cartItem.Quantity);
        }

        var createdOrder = await _orderRepository.CreateAsync(order);

        // Limpiar el carrito
        await _cartRepository.ClearCartAsync(cart.Id);

        var orderDto = MapToOrderDto(createdOrder);

        await _unitOfWork.CommitAsync();

        // Enviar email de confirmación al usuario
        if (createdOrder.User?.Email != null)
        {
            await _emailService.SendOrderConfirmationAsync(orderDto, createdOrder.User.Email);
        }

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Orden creada exitosamente",
            Data = orderDto
        };
    }
    catch (Exception ex)
    {
        await _unitOfWork.RollbackAsync();
        return ApiResponse<OrderDto>.Fail($"Error al crear la orden: {ex.Message}");
    }
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

        order.UpdateStatus(updateStatusDto.NewStatus);
        var updatedOrder = await _orderRepository.UpdateAsync(order);

        var orderDto = MapToOrderDto(updatedOrder);

        // Si el estado es 'Completed', enviar email de pago realizado
        if (updateStatusDto.NewStatus == OrderStatus.Completed && updatedOrder.User?.Email != null)
        {
            await _emailService.SendOrderPaidEmailAsync(orderDto, updatedOrder.User.Email);
        }

        return new ApiResponse<OrderDto>
        {
            Success = true,
            Message = "Estado de orden actualizado exitosamente",
            Data = orderDto
        };
    }
}
