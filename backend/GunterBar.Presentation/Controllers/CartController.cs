using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<CartDto>> GetCartById(Guid id)
    {
        var result = await _cartService.GetCartByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult<CartDto>> AddToCart(Guid id, [FromBody] CartItemDto item)
    {
        var result = await _cartService.AddItemToCartAsync(id, item);
        return Ok(result);
    }

    [HttpDelete("{id}/items/{drinkId}")]
    public async Task<ActionResult<CartDto>> RemoveFromCart(Guid id, Guid drinkId)
    {
        var result = await _cartService.RemoveItemFromCartAsync(id, drinkId);
        return Ok(result);
    }

    [HttpPost("{id}/clear")]
    public async Task<ActionResult> ClearCart(Guid id)
    {
        await _cartService.ClearCartAsync(id);
        return NoContent();
    }
}
