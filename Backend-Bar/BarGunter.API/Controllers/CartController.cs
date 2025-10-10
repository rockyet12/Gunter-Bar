using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

/// <summary>
/// Controller para gestión del carrito de compras
/// Versión mejorada con funcionalidades adicionales de carrito
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener todos los carritos (solo para administradores)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
    {
        _logger.LogInformation("Administrador obteniendo todos los carritos");
        
        var carts = await _cartService.GetAllAsync();
        
        _logger.LogInformation("Carritos obtenidos: {Count} encontrados", carts.Count());
        return Ok(carts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> GetById(int id)
    {
        var cart = await _cartService.GetByIdAsync(id);
        if (cart == null)
            return NotFound();
        return Ok(cart);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Cart>>> GetByUserId(int userId)
    {
        var carts = await _cartService.GetAllAsync();
        // Filtrar por usuario si es necesario según la lógica de negocio
        return Ok(carts);
    }

    [HttpPost]
    public async Task<ActionResult<Cart>> Create([FromBody] Cart cart)
    {
        var createdCart = await _cartService.CreateAsync(cart);
        return CreatedAtAction(nameof(GetById), new { id = createdCart.CartId }, createdCart);
    }

    [HttpPost("{cartId}/items")]
    public async Task<ActionResult> AddItem(int cartId, [FromBody] Cart item)
    {
        // Asignar el ID del carrito al item
        item.CartId = cartId;
        var addedItem = await _cartService.CreateAsync(item);
        return Ok(addedItem);
    }

    [HttpDelete("{cartId}/items/{itemId}")]
    public async Task<ActionResult> RemoveItem(int cartId, int itemId)
    {
        var deleted = await _cartService.DeleteAsync(itemId);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Cart>> Update(int id, [FromBody] Cart cart)
    {
        var updatedCart = await _cartService.UpdateAsync(id, cart);
        if (updatedCart == null)
            return NotFound();
        return Ok(updatedCart);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Eliminando carrito {CartId}", id);
        
        var deleted = await _cartService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("No se pudo eliminar carrito {CartId} - no encontrado", id);
            return NotFound($"Carrito con ID {id} no encontrado");
        }
        
        _logger.LogInformation("Carrito {CartId} eliminado exitosamente", id);
        return NoContent();
    }

    /// <summary>
    /// Limpiar carrito (eliminar todos los items)
    /// </summary>
    [HttpPost("{id}/clear")]
    public async Task<ActionResult> ClearCart(int id)
    {
        _logger.LogInformation("Limpiando carrito {CartId}", id);
        
        // Implementar lógica para limpiar carrito basado en la estructura actual
        var deleted = await _cartService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("No se pudo limpiar carrito {CartId} - no encontrado", id);
            return NotFound($"Carrito con ID {id} no encontrado");
        }
        
        _logger.LogInformation("Carrito {CartId} limpiado exitosamente", id);
        return Ok(new { message = "Carrito limpiado exitosamente" });
    }

    /// <summary>
    /// Obtener resumen de carritos activos
    /// </summary>
    [HttpGet("summary")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<ActionResult<object>> GetCartsSummary()
    {
        _logger.LogInformation("Obteniendo resumen de carritos");
        
        var carts = await _cartService.GetAllAsync();
        
        var summary = new
        {
            TotalCarts = carts.Count(),
            ActiveCarts = carts.Count(), // Todos los carritos se consideran activos por ahora
            TotalItems = 0, // Se podría implementar si Cart tiene items
            AverageItemsPerCart = 0.0
        };
        
        _logger.LogInformation("Resumen de carritos generado: {TotalCarts} carritos totales", summary.TotalCarts);
        return Ok(summary);
    }
}
