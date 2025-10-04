using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using BarGunter.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.API.Controllers;

// Controlador para la gestión de pedidos. Protegido por JWT y roles.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidoController : ControllerBase
{
    private readonly IpedidoService _pedidoService;

    public PedidoController(IpedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    // Solo administradores y empleados pueden ver todos los pedidos
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var pedidos = await _pedidoService.GetAllPedidos();
        return Ok(pedidos);
    }

    // Solo administradores y empleados pueden ver un pedido por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var pedido = await _pedidoService.GetPedidoById(id);
        if (pedido == null)
        {
            return NotFound();
        }
        return Ok(pedido);
    }

    // Clientes pueden crear pedidos
    [HttpPost]
    [Authorize(Roles = "Cliente")]
    public async Task<IActionResult> Post([FromBody] Pedido pedido)
    {
        var id = await _pedidoService.AddPedido(pedido);
        return CreatedAtAction(nameof(Get), new { id = id }, pedido);
    }

    // Solo administradores y empleados pueden actualizar el estado de un pedido
    [HttpPut("{id}/estado")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> UpdateEstado(int id, [FromQuery] EstadoPedido estado)
    {
        var result = await _pedidoService.UpdateEstado(id, estado);
        if (!result)
        {
            return NotFound(new { Message = $"No se encontró el pedido con ID {id} o el estado es el mismo." });
        }
        return NoContent();
    }
}