using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Importante para la seguridad

namespace BarGunter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Protege este controlador
public class TragoController : ControllerBase
{
    private readonly ITragoService _tragoService;

    public TragoController(ITragoService tragoService)
    {
        _tragoService = tragoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tragos = await _tragoService.GetAllTragos();
        return Ok(tragos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var trago = await _tragoService.GetTragoById(id);
        if (trago == null)
        {
            return NotFound();
        }
        return Ok(trago);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Tragos trago)
    {
        var id = await _tragoService.AddTrago(trago);
        return CreatedAtAction(nameof(Get), new { id = id }, trago);
    }

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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