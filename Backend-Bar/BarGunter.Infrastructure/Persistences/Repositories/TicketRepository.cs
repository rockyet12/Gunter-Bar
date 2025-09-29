using BarGunter.Application.Contracts.IRepositories;
using BarGunter.Domain.Entities;
using BarGunter.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarGunter.Infrastructure.Persistences.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly BarGunterDbContext _context;

    public TicketRepository(BarGunterDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllTickets()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task<Ticket> GetTicketById(int id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<int> AddTicket(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        return ticket.IdTicket;
    }
}