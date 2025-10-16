using System.Security.Claims;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

/// <summary>
/// Controlador para gestionar los pedidos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene un pedido por su ID
    /// </summary>
    /// <param name="id">ID del pedido</param>
    /// <returns>El pedido solicitado</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(int id)
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            var userRole = User.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _orderService.GetOrderByIdAsync(id, int.Parse(userId));

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el pedido {OrderId}", id);
            return StatusCode(500, new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Error interno al obtener el pedido"
            });
        }
    }

    /// <summary>
    /// Obtiene los pedidos del usuario actual
    /// </summary>
    /// <returns>Lista de pedidos del usuario</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetMyOrders()
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _orderService.GetUserOrdersAsync(int.Parse(userId));
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los pedidos del usuario");
            return StatusCode(500, new ApiResponse<IEnumerable<OrderDto>>
            {
                Success = false,
                Message = "Error interno al obtener los pedidos"
            });
        }
    }

    /// <summary>
    /// Obtiene todos los pedidos (Solo Admin)
    /// </summary>
    /// <returns>Lista de todos los pedidos</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<OrderDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAll()
    {
        try
        {
            var response = await _orderService.GetAllOrdersAsync();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los pedidos");
            return StatusCode(500, new ApiResponse<IEnumerable<OrderDto>>
            {
                Success = false,
                Message = "Error interno al obtener los pedidos"
            });
        }
    }

    /// <summary>
    /// Crea un nuevo pedido
    /// </summary>
    /// <param name="createDto">Datos del pedido a crear</param>
    /// <returns>El pedido creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> Create([FromBody] CreateOrderDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Datos inválidos",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });

        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            var response = await _orderService.CreateOrderFromCartAsync(int.Parse(userId), createDto);

            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetById), 
                new { id = response.Data?.Id ?? throw new InvalidOperationException("ID no puede ser null") }, 
                response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el pedido");
            return StatusCode(500, new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Error interno al crear el pedido"
            });
        }
    }

    /// <summary>
    /// Actualiza el estado de un pedido (Solo Admin)
    /// </summary>
    /// <param name="id">ID del pedido</param>
    /// <param name="updateDto">Datos de actualización del pedido</param>
    /// <returns>El pedido actualizado</returns>
    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Datos inválidos",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });

        try
        {
            var response = await _orderService.UpdateOrderStatusAsync(id, updateDto);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el estado del pedido {OrderId}", id);
            return StatusCode(500, new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Error interno al actualizar el estado del pedido"
            });
        }
    }

    /// <summary>
    /// Cancela un pedido
    /// </summary>
    /// <param name="id">ID del pedido</param>
    /// <returns>El pedido cancelado</returns>
    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CancelOrder(int id)
    {
        try
        {
            var userId = User.FindFirst("uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Forbid();

            // Verificar que el pedido exista y pertenezca al usuario
            var order = await _orderService.GetOrderByIdAsync(id, int.Parse(userId));
            if (!order.Success)
                return NotFound(order);

            var updateDto = new UpdateOrderStatusDto 
            { 
                OrderId = id,
                NewStatus = OrderStatus.Cancelled,
                Notes = "Orden cancelada por el usuario"
            };

            var response = await _orderService.UpdateOrderStatusAsync(id, updateDto);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar el pedido {OrderId}", id);
            return StatusCode(500, new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "Error interno al cancelar el pedido"
            });
        }
    }
}

