using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.API.Controllers;

[Route("api/[controller]")]
[Authorize] // Protege este controlador con JWT
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var categorias = await _categoriaService.GetAllCategorias();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var categoria = await _categoriaService.GetCategoriaById(id);
        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Categoria categoria)
    {
        var id = await _categoriaService.AddCategoria(categoria);
        return CreatedAtAction(nameof(Get), new { id = id }, categoria);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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