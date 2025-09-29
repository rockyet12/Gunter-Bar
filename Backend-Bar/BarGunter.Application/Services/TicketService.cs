using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Application.Contracts.IServices;
using BarGunter.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        return await _ticketRepository.GetAllTickets();
    }

    public async Task<Ticket> GetTicketById(int id)
    {
        return await _ticketRepository.GetTicketById(id);
    }

    public async Task<int> CreateTicket(Ticket ticket)
    {
        if (ticket.Total <= 0)
        {
            throw new ArgumentException("El total del ticket debe ser mayor a cero.");
        }
        return await _ticketRepository.AddTicket(ticket);
    }
}