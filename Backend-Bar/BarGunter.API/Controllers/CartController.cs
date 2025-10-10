using BarGunter.Application.Interfaces.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
    {
        var carts = await _cartService.GetAllAsync();
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
        var deleted = await _cartService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/clear")]
    public async Task<ActionResult> ClearCart(int id)
    {
        // Implementar lógica para limpiar carrito basado en la estructura actual
        var deleted = await _cartService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return Ok();
    }
}
