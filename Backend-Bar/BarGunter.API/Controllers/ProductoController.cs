using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    // Solo administradores y empleados pueden ver todos los productos
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var productos = await _productoService.GetAllProductos();
        return Ok(productos);
    }

    // Solo administradores y empleados pueden ver un producto por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var producto = await _productoService.GetProductoById(id);
        if (producto == null)
        {
            return NotFound();
        }
        return Ok(producto);
    }

    // Solo administradores pueden crear productos
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Post([FromBody] Producto producto)
    {
        var id = await _productoService.AddProducto(producto);
        return CreatedAtAction(nameof(Get), new { id = id }, producto);
    }

    // Solo administradores pueden modificar productos
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Put(int id, [FromBody] Producto producto)
    {
        if (id != producto.CDProducto)
        {
            return BadRequest();
        }
        var result = await _productoService.UpdateProducto(producto);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // Solo administradores pueden eliminar productos
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productoService.DeleteProducto(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
