using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.API.Controllers;

// Controlador para la gestión de categorías. Protegido por JWT y roles.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // Solo administradores y empleados pueden ver todas las categorías
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var categorias = await _categoriaService.GetAllCategorias();
        return Ok(categorias);
    }

    // Solo administradores y empleados pueden ver una categoría por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var categoria = await _categoriaService.GetCategoriaById(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    // Solo administradores pueden crear categorías
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Post([FromBody] Categoria categoria)
    {
        var id = await _categoriaService.AddCategoria(categoria);
        return CreatedAtAction(nameof(Get), new { id = id }, categoria);
    }

    // Solo administradores pueden modificar categorías
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Put(int id, [FromBody] Categoria categoria)
    {
        if (id != categoria.IdCategoria)
        {
            return BadRequest();
        }
        var result = await _categoriaService.UpdateCategoria(categoria);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // Solo administradores pueden eliminar categorías
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoriaService.DeleteCategoria(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}