using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.API.Controllers;

[Route("api/[controller]")]
[Authorize] // Protege este controlador con JWT
[ApiController]
public class PedidoController : ControllerBase
{
    private readonly IpedidoService _pedidoService;

    public PedidoController(IpedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var pedidos = await _pedidoService.GetAllPedidos();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var pedido = await _pedidoService.GetPedidoById(id);
        if (pedido == null)
        {
            return NotFound();
        }
        return Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Pedido pedido)
    {
        var id = await _pedidoService.AddPedido(pedido);
        return CreatedAtAction(nameof(Get), new { id = id }, pedido);
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> UpdateEstado(int id, [FromQuery] EstadoPedido estado)
    {
        var result = await _pedidoService.UpdateEstado(id, estado);
        if (!result)
        {
            return NotFound(new { Message = $"No se encontró el pedido con ID {id} o el estado es el mismo." });
        }
        return NoContent(); // 204 No Content para una actualización exitosa
    }
}