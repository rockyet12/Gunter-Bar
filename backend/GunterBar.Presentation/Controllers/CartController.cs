using System.Security.Claims;
using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GunterBar.Presentation.Controllers;

/// <summary>
/// Controlador para gestionar el carrito de compras
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;

    public CartController(ICartService cartService, ILogger<CartController> logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene el carrito del usuario actual
    /// </summary>
    /// <returns>El carrito del usuario</returns>
    /// <summary>
    /// Obtiene el carrito del usuario actual
    /// </summary>
    /// <returns>El carrito del usuario</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartDto>> GetMyCart()
    {
        var userId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Forbid();

        try
        {
            var response = await _cartService.GetCartAsync(int.Parse(userId));
            if (!response.Success)
                return NotFound(response.Message);
                
            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el carrito del usuario {UserId}", userId);
            return StatusCode(500, "Error interno al obtener el carrito");
        }
    }

    /// <summary>
    /// Agrega un item al carrito del usuario
    /// </summary>
    /// <param name="addToCartDto">Datos del item a agregar</param>
    /// <returns>El carrito actualizado</returns>
    [HttpPost("items")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CartDto>> AddToCart([FromBody] AddToCartDto addToCartDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Forbid();

        try
        {
            var response = await _cartService.AddToCartAsync(int.Parse(userId), addToCartDto);
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar item al carrito del usuario {UserId}", userId);
            return StatusCode(500, "Error interno al agregar item al carrito");
        }
    }

    /// <summary>
    /// Actualiza la cantidad de un item en el carrito
    /// </summary>
    /// <param name="itemId">ID del item</param>
    /// <param name="updateDto">Datos de actualización</param>
    /// <returns>El carrito actualizado</returns>
    [HttpPut("items/{itemId:int}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartDto>> UpdateCartItem(int itemId, [FromBody] UpdateCartItemDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Forbid();

        try
        {
            var response = await _cartService.UpdateCartItemAsync(int.Parse(userId), itemId, updateDto);
            if (!response.Success)
                return response.Message.Contains("encontrado") ? NotFound(response.Message) : BadRequest(response.Message);

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar item {ItemId} del carrito del usuario {UserId}", itemId, userId);
            return StatusCode(500, "Error interno al actualizar item del carrito");
        }
    }

    /// <summary>
    /// Elimina un item del carrito
    /// </summary>
    /// <param name="itemId">ID del item a eliminar</param>
    /// <returns>200 OK si se eliminó correctamente</returns>
    [HttpDelete("items/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveFromCart(int itemId)
    {
        var userId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Forbid();

        try
        {
            var response = await _cartService.RemoveFromCartAsync(int.Parse(userId), itemId);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar item {ItemId} del carrito del usuario {UserId}", itemId, userId);
            return StatusCode(500, "Error interno al eliminar item del carrito");
        }
    }

    /// <summary>
    /// Vacía el carrito del usuario
    /// </summary>
    /// <returns>200 OK si se vació correctamente</returns>
    [HttpPost("clear")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ClearCart()
    {
        var userId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Forbid();

        try
        {
            var response = await _cartService.ClearCartAsync(int.Parse(userId));
            if (!response.Success)
                return NotFound(response.Message);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al vaciar el carrito del usuario {UserId}", userId);
            return StatusCode(500, "Error interno al vaciar el carrito");
        }
    }
}
