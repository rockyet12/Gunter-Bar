using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipoController : ControllerBase
{
    private readonly ITipoService _tipoService;

    public TipoController(ITipoService tipoService)
    {
        _tipoService = tipoService;
    }

    // Solo administradores y empleados pueden ver todos los tipos
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var tipos = await _tipoService.GetAllTipos();
        return Ok(tipos);
    }

    // Solo administradores y empleados pueden ver un tipo por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var tipo = await _tipoService.GetTipoById(id);
        if (tipo == null)
        {
            return NotFound();
        }
        return Ok(tipo);
    }

    // Solo administradores pueden crear tipos
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Post([FromBody] Tipo tipo)
    {
        var id = await _tipoService.AddTipo(tipo);
        return CreatedAtAction(nameof(Get), new { id = id }, tipo);
    }

    // Solo administradores pueden modificar tipos
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Put(int id, [FromBody] Tipo tipo)
    {
        if (id != tipo.IdTipo)
        {
            return BadRequest();
        }
        var result = await _tipoService.UpdateTipo(tipo);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // Solo administradores pueden eliminar tipos
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _tipoService.DeleteTipo(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
