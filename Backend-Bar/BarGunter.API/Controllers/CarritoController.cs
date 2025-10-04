using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarritoController : ControllerBase
{
    private readonly ICarritoService _carritoService;

    public CarritoController(ICarritoService carritoService)
    {
        _carritoService = carritoService;
    }

    // Solo clientes pueden ver su carrito
    [HttpGet]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Get()
    {
        var carritos = await _carritoService.GetAllCarritos();
        return Ok(carritos);
    }

    // Solo clientes pueden ver un carrito por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Get(int id)
    {
        var carrito = await _carritoService.GetCarritoById(id);
        if (carrito == null)
        {
            return NotFound();
        }
        return Ok(carrito);
    }

    // Solo clientes pueden agregar productos a su carrito
    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Post([FromBody] Carrito carrito)
    {
        var id = await _carritoService.AddCarrito(carrito);
        return CreatedAtAction(nameof(Get), new { id = id }, carrito);
    }

    // Solo clientes pueden modificar su carrito
    [HttpPut("{id}")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Put(int id, [FromBody] Carrito carrito)
    {
        if (id != carrito.IdCarrito)
        {
            return BadRequest();
        }
        var result = await _carritoService.UpdateCarrito(carrito);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // Solo clientes pueden eliminar su carrito
    [HttpDelete("{id}")]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _carritoService.DeleteCarrito(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
