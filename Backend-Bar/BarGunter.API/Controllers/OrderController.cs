using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByUserId(int userId)
    {
        var orders = await _orderService.GetAllAsync();
        // Filtrar por usuario según la lógica de negocio
        return Ok(orders);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByStatus(string status)
    {
        var orders = await _orderService.GetAllAsync();
        // Filtrar por status según la lógica de negocio
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
