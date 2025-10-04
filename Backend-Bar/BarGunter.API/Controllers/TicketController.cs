using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.API.Controllers;

// Controlador para la gestión de tickets. Protegido por JWT y roles.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // Solo administradores y empleados pueden ver todos los tickets
    [HttpGet]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get()
    {
        var tickets = await _ticketService.GetAllTickets();
        return Ok(tickets);
    }

    // Solo administradores y empleados pueden ver un ticket por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Get(int id)
    {
        var ticket = await _ticketService.GetTicketById(id);
        if (ticket == null)
        {
            return NotFound();
        }
        return Ok(ticket);
    }

    // Solo administradores y empleados pueden crear tickets
    [HttpPost]
    [Authorize(Roles = "Administrador,Vendedor")]
    public async Task<IActionResult> Post([FromBody] Ticket ticket)
    {
        try
        {
            var id = await _ticketService.CreateTicket(ticket);
            return CreatedAtAction(nameof(Get), new { id = id }, ticket);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}