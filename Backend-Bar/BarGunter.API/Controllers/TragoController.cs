using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Importante para la seguridad

namespace BarGunter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
// Controlador para la gestión de tragos. Protegido por JWT y roles.
[Authorize]
public class TragoController : ControllerBase
{
    private readonly ITragoService _tragoService;

    public TragoController(ITragoService tragoService)
    {
        _tragoService = tragoService;
    }

    // Solo administradores y empleados pueden ver todos los tragos
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var tragos = await _tragoService.GetAllTragos();
        return Ok(tragos);
    }

    // Solo administradores y empleados pueden ver un trago por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var trago = await _tragoService.GetTragoById(id);
        if (trago == null)
        {
            return NotFound();
        }
        return Ok(trago);
    }

    // Solo administradores pueden crear tragos
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Post([FromBody] Tragos trago)
    {
        var id = await _tragoService.AddTrago(trago);
        return CreatedAtAction(nameof(Get), new { id = id }, trago);
    }

    // Solo administradores pueden modificar tragos
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Put(int id, [FromBody] Tragos trago)
    {
        if (id != trago.IdTragos)
        {
            return BadRequest();
        }
        var result = await _tragoService.UpdateTrago(trago);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // Solo administradores pueden eliminar tragos
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _tragoService.DeleteTrago(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}