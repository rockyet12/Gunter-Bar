using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

/// <summary>
/// Controller para gestión de órdenes del bar
/// Versión mejorada con funcionalidades de filtrado y estadísticas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todas las órdenes (Admin/Employee only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll(
        [FromQuery] string? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        _logger.LogInformation("Obteniendo órdenes con filtros: status={Status}, fromDate={FromDate}, toDate={ToDate}", 
            status, fromDate, toDate);

        var orders = await _orderService.GetAllAsync();

        // Aplicar filtros si se proporcionan
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, true, out var statusEnum))
        {
            orders = orders.Where(o => o.Status == statusEnum);
        }

        if (fromDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate <= toDate.Value);
        }

        var result = orders.ToList();
        _logger.LogInformation("Órdenes filtradas: {Count} encontradas", result.Count);
        
        return Ok(result);
    }

    /// <summary>
    /// Obtener órdenes por estado específico
    /// </summary>
    [HttpGet("by-status/{status}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByStatus(string status)
    {
        _logger.LogInformation("Obteniendo órdenes con estado: {Status}", status);

        if (!Enum.TryParse<OrderStatus>(status, true, out var statusEnum))
        {
            _logger.LogWarning("Estado de orden inválido: {Status}", status);
            return BadRequest($"Estado '{status}' no es válido. Estados válidos: {string.Join(", ", Enum.GetNames<OrderStatus>())}");
        }

        var orders = await _orderService.GetAllAsync();
        var filteredOrders = orders.Where(o => o.Status == statusEnum).ToList();

        _logger.LogInformation("Órdenes con estado {Status}: {Count} encontradas", status, filteredOrders.Count);
        return Ok(filteredOrders);
    }

    /// <summary>
    /// Obtener órdenes recientes (últimos 7 días)
    /// </summary>
    [HttpGet("recent")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetRecentOrders([FromQuery] int days = 7)
    {
        _logger.LogInformation("Obteniendo órdenes recientes de los últimos {Days} días", days);

        var cutoffDate = DateTime.Now.AddDays(-days);
        var orders = await _orderService.GetAllAsync();
        var recentOrders = orders
            .Where(o => o.OrderDate >= cutoffDate)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        _logger.LogInformation("Órdenes recientes encontradas: {Count}", recentOrders.Count);
        return Ok(recentOrders);
    }

    /// <summary>
    /// Obtener mis órdenes (usuario autenticado)
    /// </summary>
    [HttpGet("my-orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
        {
            _logger.LogWarning("Token inválido al obtener órdenes del usuario");
            return Unauthorized("Token inválido");
        }

        _logger.LogInformation("Obteniendo órdenes del usuario {UserId}", userId);

        var orders = await _orderService.GetAllAsync();
        // Nota: Necesitaríamos una propiedad UserId en Order para filtrar correctamente
        // Por ahora devolvemos todas las órdenes para el usuario
        var userOrders = orders.OrderByDescending(o => o.OrderDate).ToList();

        _logger.LogInformation("Órdenes del usuario {UserId}: {Count} encontradas", userId, userOrders.Count);
        return Ok(userOrders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        _logger.LogInformation("Obteniendo orden {OrderId}", id);
        
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            _logger.LogWarning("Orden {OrderId} no encontrada", id);
            return NotFound($"Orden con ID {id} no encontrada");
        }
        
        _logger.LogInformation("Orden {OrderId} obtenida exitosamente", id);
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByUserId(int userId)
    {
        _logger.LogInformation("Obteniendo órdenes del usuario {UserId}", userId);
        
        var orders = await _orderService.GetAllAsync();
        // En un sistema real, se filtraría por relación User -> Ticket -> Order
        // Por simplicidad, retornamos todas las órdenes por ahora
        
        _logger.LogInformation("Órdenes obtenidas: {Count}", orders.Count());
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] Order order)
    {
        var createdOrder = await _orderService.CreateAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
    }

    [HttpPost("from-cart/{cartId}")]
    public async Task<ActionResult<Order>> CreateFromCart(int cartId)
    {
        // Implementar lógica para crear orden desde carrito
        var order = new Order(); // Lógica básica por ahora
        var createdOrder = await _orderService.CreateAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> Update(int id, [FromBody] Order order)
    {
        var updatedOrder = await _orderService.UpdateAsync(id, order);
        if (updatedOrder == null)
            return NotFound();
        return Ok(updatedOrder);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        // Implementar lógica para actualizar status
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _orderService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelOrder(int id)
    {
        // Implementar lógica para cancelar orden
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok();
    }
}
